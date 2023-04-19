using Simulation.Agents;

namespace Simulation;

public class MainSimulation
{
    private const float LoadingUpdateDelta = 0.1f;
    
    private readonly SimulationConfig _simulationConfig;
    private readonly GridSpace _gridSpace;
    private readonly List<Scout> _scouts;
    private readonly List<Worker> _workers;

    public MainSimulation(SimulationConfig simulationConfig)
    {
        _simulationConfig = simulationConfig;
        _gridSpace = new GridSpace(_simulationConfig.MapConfig.MapSize.Width,
            _simulationConfig.MapConfig.MapSize.Height);

        
        for (var x = 0; x < _simulationConfig.MapConfig.MapSize.Width; x++)
        {
            for (var y = 0; y < _simulationConfig.MapConfig.MapSize.Width; y++)
            {
                if (simulationConfig.MapConfig.Map[x, y] == 1)
                    _gridSpace.AddAgent(new Drone());
            }
        }

        var startX = _simulationConfig.MapConfig.StartDronePoint.X;
        var startY = _simulationConfig.MapConfig.StartDronePoint.Y;
        _gridSpace.AddAgent(new StartDrone(0, startX, startY), startX, startY);
        
        var endX = _simulationConfig.MapConfig.EndDronePoint.X;
        var endY = _simulationConfig.MapConfig.EndDronePoint.Y;
        _gridSpace.AddAgent(new EndDrone(1, endX, endY), endX, endY);
        
        
        
        for (var i = 0; i < _simulationConfig.DroneCount; i++)
        {
            var x = new Random().Next(0, _simulationConfig.GridWidth);
            var y = new Random().Next(0, _simulationConfig.GridHeight);
            _gridSpace.AddAgent(new Drone(i, x, y), x, y);
        }
        
        
        _scouts = new List<Scout>();
        for (var i = 0; i < _simulationConfig.ScoutCount; i++)
        {
            var scout = new Scout(i, dronePathConfig.StartDrone, 1, 0, 100, 20, dronePathConfig);
            _scouts.Add(scout);
        }

        _workers = new List<Worker>();
        SimulationEventQueue = new SimulationEventQueue();

        foreach (var drone in _gridSpace.GetAgents().Select(agent => agent as Drone))
        {
            SimulationEventQueue.Enqueue(new SimulationEvent(SimulationEventQueue.CurrentTick + 1,
                () => drone?.DoAction(_gridSpace)));
        }

        var updateLoadingProcessesEvent = new SimulationEvent(SimulationEventQueue.CurrentTick + LoadingUpdateDelta,
            UpdateLoadingProcesses);
        SimulationEventQueue.Enqueue(updateLoadingProcessesEvent);
    }
    
    public SimulationEventQueue SimulationEventQueue { get; }

    private void UpdateLoadingProcesses()
    {
        var drones = _gridSpace.GetAgents().Cast<Drone>().ToList();
        foreach (var drone in drones)
        {
            foreach (var dataLoadingProcess in drone.DataLoadingProcesses)
            {
                dataLoadingProcess.DoLoadProcess(LoadingUpdateDelta);
                
                if (!dataLoadingProcess.IsLoadingEnded)
                    continue;

                var agentEvent = new SimulationEvent(SimulationEventQueue.CurrentTick,
                    () => dataLoadingProcess.DataAgent.DoAction(drones));
                SimulationEventQueue.Enqueue(agentEvent);
            }

            drone.DataLoadingProcesses = drone.DataLoadingProcesses.Where(dlp => !dlp.IsLoadingEnded).ToList();
        }

        var updateLoadingProcessesEvent = new SimulationEvent(SimulationEventQueue.CurrentTick + LoadingUpdateDelta, UpdateLoadingProcesses);
        SimulationEventQueue.Enqueue(updateLoadingProcessesEvent);
    }

    public void Run()
    {
        while (SimulationEventQueue.Count > 0)
        {
            var simulationEvent = SimulationEventQueue.Dequeue();
            simulationEvent?.Execute();
        }
    }
}