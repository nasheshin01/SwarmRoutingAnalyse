namespace Simulation.Agents;

public abstract class DataAgent : Agent
{
    protected DataAgent(int id, Drone currentDrone, int dataSize, float instantiateTime) : base(id)
    {
        CurrentDrone = currentDrone;
        DataSize = dataSize;
        InstantiateTime = instantiateTime;
    }
    
    public Drone CurrentDrone { get; set; }
    public int DataSize { get; set; }
    public float InstantiateTime { get; set; }

    public void LoadToDrone(Drone drone)
    {
        var dataLoadingProcess = new DataLoadingProcess(this, CurrentDrone, drone);
        drone.DataLoadingProcesses.Enqueue(dataLoadingProcess);

        CurrentDrone = drone;
    }
}