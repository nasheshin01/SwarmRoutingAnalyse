namespace Simulation.Agents;

public class WorkerCreator : Agent
{
    private readonly StartDrone _startDrone;
    private readonly SimulationConfig _simulationConfig;
    private readonly List<Data> _unsentDatas;
    private readonly List<Worker> _workers;

    private int _currentDataId;
    private int _currentWorkerId;

    public WorkerCreator(StartDrone startDrone, SimulationConfig simulationConfig, int id) : base(id)
    {
        _startDrone = startDrone;
        _simulationConfig = simulationConfig;

        _unsentDatas = new List<Data>();
        _currentDataId = 0;

        _workers = new List<Worker>();
    }

    public void DoAction(int tick)
    {
        _unsentDatas.Add(new Data(_currentDataId, _simulationConfig.DataSize));
        _currentDataId++;

        if (!_startDrone.IsSuitablePathExists)
            return;

        var packagesToSend = _unsentDatas.SelectMany(d => d.Packages).ToList();
        var packageIndex = 0;
        while (packageIndex < packagesToSend.Count)
        {
            foreach (var suitablePath in _startDrone.SuitablePaths)
            {
                var currentPackage = packagesToSend[packageIndex];
                packageIndex++;

                var worker = new Worker(_currentWorkerId, _startDrone, _simulationConfig.PackageSize, tick,
                    suitablePath, currentPackage);
                _workers.Add(worker);
                _currentWorkerId++;
            }
        }
    }

    public List<Worker> ExtractCreatedWorkers()
    {
        var result = new List<Worker>();
        result.AddRange(_workers);
        _workers.Clear();
        
        return result;
    }
}