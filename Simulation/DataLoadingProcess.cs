using Simulation.Agents;

namespace Simulation;

public class DataLoadingProcess
{
    private float _loadedData;
    
    public DataLoadingProcess(DataAgent dataAgent, Drone droneFrom, Drone droneTo)
    {
        DataAgent = dataAgent;
        DroneFrom = droneFrom;
        DroneTo = droneTo;
        
        _loadedData = 0f;
    }

    public DataAgent DataAgent { get; set; }
    public Drone DroneFrom { get; set; }
    public Drone DroneTo { get; set; }
    public bool IsLoadingEnded { get; set; }

    public void DoLoadProcess(float elapsedTime, float constLoadingSpeed)
    {
        var loadingSpeed = (float) (constLoadingSpeed / Math.Pow(Utils.GetDroneDistance(DroneFrom, DroneTo), 2));
        _loadedData += loadingSpeed * elapsedTime;

        IsLoadingEnded = _loadedData > DataAgent.DataSize;
        DataAgent.LastLoadingTime += elapsedTime;
    }
}