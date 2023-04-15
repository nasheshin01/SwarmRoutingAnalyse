using Simulation.Agents;

namespace Simulation;

public class GridSpace
{
    public GridSpace(int width, int height)
    {
        Width = width;
        Height = height;

        Grid = new List<Agent>[width, height];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Grid[x, y] = new List<Agent>();
            }
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public List<Agent>[,] Grid { get; set; }

    public void MoveAgent()
    {
        throw new NotImplementedException();
    }
}