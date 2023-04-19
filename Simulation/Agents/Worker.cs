namespace Simulation.Agents;

public class Worker : DataAgent
{
    public Worker(int id, Drone currentDrone, int dataSize, float instantiateTime, List<Drone> path,
        Package package) : base(id, currentDrone, dataSize, instantiateTime)
    {
        WorkerState = WorkerState.Sending;
        Path = path;
        Package = package;
    }

    public WorkerState WorkerState { get; set; }
    public List<Drone> Path { get; set; }
    public Package Package { get; set; }

    public override void DoAction(List<Drone> drones)
    {
        if (WorkerState == WorkerState.Sending && CurrentDrone == Path[^1])
            WorkerState = WorkerState.GoingToStart;
        if (WorkerState == WorkerState.GoingToStart && CurrentDrone == Path[0])
            WorkerState = WorkerState.SendingEnded;

        if (WorkerState == WorkerState.Sending)
        {
            Drone? nextDrone = null;
            for (var i = 0; i < Path.Count; i++)
            {
                if (Path[i] != CurrentDrone)
                    continue;

                nextDrone = Path[i + 1];
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
            for (var i = 0; i < Path.Count; i++)
            {
                if (Path[i] != CurrentDrone)
                    continue;

                nextDrone = Path[i - 1];
                break;
            }

            if (nextDrone == null)
            {
                DestroyAgent();
                return;
            }

            LoadToDrone(nextDrone);
        }
        else if (WorkerState == WorkerState.SendingEnded)
        {
            //Give data about path
        }
    }
}