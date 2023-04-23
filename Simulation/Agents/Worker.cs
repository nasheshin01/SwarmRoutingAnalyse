namespace Simulation.Agents;

public class Worker : DataAgent
{
    public Worker(int id, Drone currentDrone, int dataSize, float instantiateTime, Path path,
        Package package) : base(id, currentDrone, dataSize, instantiateTime)
    {
        WorkerState = WorkerState.Sending;
        Path = path;
        Package = package;
    }

    public WorkerState WorkerState { get; set; }
    public Path Path { get; set; }
    public Package Package { get; set; }
    public float WayBackLoadingTime { get; set; }

    public override void DoAction(List<Drone> drones)
    {
        if (WorkerState == WorkerState.Sending && CurrentDrone == Path.Drones[^1])
            WorkerState = WorkerState.GoingToStart;
        if (WorkerState == WorkerState.GoingToStart && CurrentDrone == Path.Drones[0])
            WorkerState = WorkerState.SendingEnded;

        if (WorkerState == WorkerState.Sending)
        {
            Drone? nextDrone = null;
            for (var i = 0; i < Path.Drones.Count; i++)
            {
                if (Path.Drones[i] != CurrentDrone)
                    continue;

                nextDrone = Path.Drones[i + 1];
                break;
            }
            
            if (nextDrone == null)
            {
                DestroyAgent();
                return;
            }

            LoadToDrone(nextDrone);
        }
        else if (WorkerState == WorkerState.GoingToStart)
        {
            Drone? nextDrone = null;
            for (var i = 0; i < Path.Drones.Count; i++)
            {
                if (Path.Drones[i] != CurrentDrone)
                    continue;

                nextDrone = Path.Drones[i - 1];
                break;
            }

            if (nextDrone == null)
            {
                DestroyAgent();
                return;
            }

            LoadToDrone(nextDrone);
            
            WayBackLoadingTime += LastLoadingTime;
        }
        else if (WorkerState == WorkerState.SendingEnded)
        {
            if (CurrentDrone is StartDrone startDrone)
                startDrone.UpdateSuitablePath(Path.Id, WayBackLoadingTime);

            DestroyAgent();
        }
        
        base.DoAction(drones);
    }
}