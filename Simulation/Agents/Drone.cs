namespace Simulation.Agents;

public class Drone : Agent
{
    public Drone(int id, int x, int y) : base(id)
    {
        X = x;
        Y = y;

        DataLoadingProcesses = new Queue<DataLoadingProcess>();
    }

    public int X { get; set; }
    public int Y { get; set; }
    public Queue<DataLoadingProcess> DataLoadingProcesses { get; set; }
}