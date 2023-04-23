using System.Drawing;

namespace Simulation.Agents;

public class Drone : Agent
{
    public Drone(int id, int x, int y) : base(id)
    {
        X = x;
        Y = y;

        DataLoadingProcesses = new List<DataLoadingProcess>();
    }

    public virtual void DoAction(Size boundaries)
    {
        var dx = new Random().Next(-1, 2);
        var dy = new Random().Next(-1, 2);
        X += dx;
        Y += dy;

        if (X < 0)
            X = 0;
        if (X >= boundaries.Width)
            X = boundaries.Width - 1;
        if (Y < 0)
            Y = 0;
        if (Y >= boundaries.Height)
            Y = boundaries.Height - 1;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public List<DataLoadingProcess> DataLoadingProcesses { get; set; }
}