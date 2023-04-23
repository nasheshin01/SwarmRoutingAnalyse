using Simulation.Agents;

namespace Simulation;

public class MainSimulation
{
    private const float LoadingUpdateDelta = 0.1f;
    
    private readonly SimulationConfig _simulationConfig;
    private readonly List<Scout> _scouts;
    private readonly List<Worker> _workers;

    private readonly StartDrone _startDrone;
    private readonly EndDrone _endDrone;
    private readonly List<Drone> _drones;
    private readonly WorkerCreator _workerCreator;
    private readonly SimulationEventQueue _simulationEventQueue;

    private int _scoutId;

    public MainSimulation(SimulationConfig simulationConfig)
    {
        _simulationEventQueue = new SimulationEventQueue();
        
        _simulationConfig = simulationConfig;

        _drones = new List<Drone>();
        for (var x = 0; x < _simulationConfig.MapConfig.MapSize.Width; x++)
        {
            for (var y = 0; y < _simulationConfig.MapConfig.MapSize.Width; y++)
            {
                if (simulationConfig.MapConfig.Map[x, y] == 1)
                {
                    var drone = new Drone(_drones.Count + 2, x, y);
                    _drones.Add(drone);
                }
                else if (simulationConfig.MapConfig.Map[x, y] == 2)
                {
                    _startDrone = new StartDrone(0, x, y, _simulationConfig.ConstLoadingSpeed);
                    _drones.Add(_startDrone);
                }
                else if (simulationConfig.MapConfig.Map[x, y] == 3)
                {
                    _endDrone = new EndDrone(1, x, y);
                    _drones.Add(_endDrone);
                }
            }
        }

        _scouts = new List<Scout>();
        for (var i = 0; i < _simulationConfig.ScoutCount; i++)
        {
            CreateScout();
        }

        _workers = new List<Worker>();
        _workerCreator = new WorkerCreator(_startDrone, _simulationConfig, 0);
        _simulationEventQueue.Enqueue(new SimulationEvent(0, WorkerCreatorDoAction));
        

        foreach (var drone in _drones.Select(agent => agent))
        {
            _simulationEventQueue.Enqueue(new SimulationEvent(0,
                () => DroneDoAction(drone)));
        }

        var updateLoadingProcessesEvent = new SimulationEvent(_simulationEventQueue.CurrentTick + LoadingUpdateDelta,
            UpdateLoadingProcesses);
        _simulationEventQueue.Enqueue(updateLoadingProcessesEvent);
    }

    private void CreateScout()
    {
        var scout = new Scout(_scoutId, _startDrone, _simulationConfig.ScoutSize, 0, _simulationConfig.ScoutEnergyLimit,
            _simulationConfig.MaxDroneDistance);
        _scouts.Add(scout);
        _simulationEventQueue.Enqueue(new SimulationEvent(0, () => ScoutDoAction(scout)));
        _scoutId++;
    }

    public event EventHandler<SimulationLogEventArgs> LogEventHandler;

    private void WorkerCreatorDoAction()
    {
        _workerCreator.DoAction(_simulationEventQueue.CurrentTick);
        var createdWorkers = _workerCreator.ExtractCreatedWorkers();
        _workers.AddRange(createdWorkers);
        foreach (var worker in createdWorkers)
        {
            LogEventHandler.Invoke(this,
                new SimulationLogEventArgs($"Package {worker.Package.Id} of data {worker.Package.ParentData.Id} was sent.",
                    _simulationEventQueue.CurrentTick, LogType.PackageSending));
        }

        foreach (var createdWorker in createdWorkers)
        {
            var simulationEvent = new SimulationEvent(_simulationEventQueue.CurrentTick, () => WorkerDoAction(createdWorker));
            _simulationEventQueue.Enqueue(simulationEvent);
        }

        var workerCreatorEvent =
            new SimulationEvent(_simulationEventQueue.CurrentTick + _simulationConfig.DataGeneratePeriod,
                WorkerCreatorDoAction);
        _simulationEventQueue.Enqueue(workerCreatorEvent);
    }
    
    private void DroneDoAction(Drone drone)
    {
        drone.DoAction(_simulationConfig.MapConfig.MapSize);
        _simulationEventQueue.Enqueue(new SimulationEvent(
            _simulationEventQueue.CurrentTick + _simulationConfig.DroneMovePeriod, () => DroneDoAction(drone)));
    }

    private void ScoutDoAction(Scout scout)
    {
        scout.DoAction(_drones);
        LogEventHandler.Invoke(this,
            new SimulationLogEventArgs($"Scout {scout.Id} is on drone {scout.CurrentDrone.Id}",
                _simulationEventQueue.CurrentTick, LogType.ScoutMove));

        if (_startDrone.SuitablePathsUpdated)
        {
            LogEventHandler.Invoke(this,
                new SimulationLogEventArgs($"Suitable paths updated, current count - {_startDrone.SuitablePaths.Count}",
                    _simulationEventQueue.CurrentTick, LogType.PathFound));
            _startDrone.SuitablePathsUpdated = false;
        }
        
        if (!scout.IsDestroyed)
            return;
        
        LogEventHandler.Invoke(this,
            new SimulationLogEventArgs($"Scout {scout.Id} destroyed",
                _simulationEventQueue.CurrentTick, LogType.ScoutMove));

        _scouts.Remove(scout);
        CreateScout();
    }
    
    private void WorkerDoAction(Worker worker)
    {
        var isPackageNotSent = worker.WorkerState == WorkerState.Sending;
        worker.DoAction(_drones);
        
        LogEventHandler.Invoke(this,
            new SimulationLogEventArgs($"Worker {worker.Id} is on drone {worker.CurrentDrone.Id}",
                _simulationEventQueue.CurrentTick, LogType.WorkerMove));

        if (isPackageNotSent && worker.WorkerState == WorkerState.GoingToStart)
        {
            LogEventHandler.Invoke(this,
                new SimulationLogEventArgs($"Package {worker.Package.Id} of data {worker.Package.ParentData.Id} was delivered. Time to deliver {_simulationEventQueue.CurrentTick - worker.InstantiateTime}",
                    _simulationEventQueue.CurrentTick, LogType.PackageSending));
        }
        
        if (!worker.IsDestroyed)
            return;

        _workers.Remove(worker);
    }

    private void UpdateLoadingProcesses()
    {
        foreach (var drone in _drones)
        {
            var dataLoadingProcess = drone.DataLoadingProcesses.FirstOrDefault();
            if (dataLoadingProcess == null)
                continue;
            
            dataLoadingProcess.DoLoadProcess(LoadingUpdateDelta, _simulationConfig.ConstLoadingSpeed);
                
            if (!dataLoadingProcess.IsLoadingEnded)
                continue;

            var dataAgent = dataLoadingProcess.DataAgent;
            switch (dataAgent)
            {
                case Scout scout:
                {
                    var agentEvent = new SimulationEvent(_simulationEventQueue.CurrentTick, () => ScoutDoAction(scout));
                    _simulationEventQueue.Enqueue(agentEvent);
                    break;
                }
                case Worker worker:
                {
                    var agentEvent = new SimulationEvent(_simulationEventQueue.CurrentTick, () => WorkerDoAction(worker));
                    _simulationEventQueue.Enqueue(agentEvent);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException("Unknown agent type");
            }

            drone.DataLoadingProcesses.RemoveAt(0);
        }

        var updateLoadingProcessesEvent = new SimulationEvent(_simulationEventQueue.CurrentTick + LoadingUpdateDelta, UpdateLoadingProcesses);
        _simulationEventQueue.Enqueue(updateLoadingProcessesEvent);
    }

    public void Run()
    {
        var evenHundredTickLast = 0;
        
        while (_simulationEventQueue.Count > 0)
        {
            if (_simulationEventQueue.CurrentTick >= _simulationConfig.EndSimulationTick)
                break;

            if (_simulationEventQueue.CurrentTick / 100f > evenHundredTickLast)
            {
                LogEventHandler.Invoke(this,
                    new SimulationLogEventArgs($"Passed abother hundred of ticks",
                        _simulationEventQueue.CurrentTick, LogType.Info));
                evenHundredTickLast++;
            }
            
            var simulationEvent = _simulationEventQueue.Dequeue();
            simulationEvent?.Execute();
        }
    }
}