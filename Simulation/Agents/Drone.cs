namespace Simulation.Agents;

public class Drone : Agent
{
    public Drone(int id, int x, int y) : base(id)
    {
        X = x;
        Y = y;

        DataLoadingProcesses = new List<DataLoadingProcess>();
    }

    public virtual void DoAction(GridSpace grid)
    {
        var dx = new Random().Next(-1, 2);
        var dy = new Random().Next(-1, 2);
        grid.MoveAgent(this, X + dx, Y + dy);
    }

    public override void MoveAgentInSpace(int x, int y)
    {
        base.MoveAgentInSpace(x, y);
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public List<DataLoadingProcess> DataLoadingProcesses { get; set; }
}