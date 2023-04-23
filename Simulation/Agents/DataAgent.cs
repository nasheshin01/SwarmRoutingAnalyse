namespace Simulation.Agents;

public abstract class DataAgent : Agent
{
    protected DataAgent(int id, Drone currentDrone, int dataSize, float instantiateTime) : base(id)
    {
        CurrentDrone = currentDrone;
        DataSize = dataSize;
        InstantiateTime = instantiateTime;

        LastLoadingTime = 0;
    }
    
    public Drone CurrentDrone { get; set; }
    public int DataSize { get; set; }
    public float InstantiateTime { get; set; }
    public float LastLoadingTime { get; set; }

    public virtual void DoAction(List<Drone> drones)
    {
        LastLoadingTime = 0;
    }
    
    public void LoadToDrone(Drone drone)
    {
        var dataLoadingProcess = new DataLoadingProcess(this, CurrentDrone, drone);
        drone.DataLoadingProcesses.Add(dataLoadingProcess);

        CurrentDrone = drone;
    }
}