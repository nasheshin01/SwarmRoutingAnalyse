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

    public void MoveAgent(Agent agent, int newX, int newY)
    {
        if (newX >= Width)
            newX = Width - 1;
        if (newX < 0)
            newX = 0;
        if (newY >= Height)
            newY = Height - 1;
        if (newY < 0)
            newY = 0;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var agentIndex = Grid[x, y].IndexOf(agent);
                if (agentIndex == -1)
                    continue;

                Grid[x, y].RemoveAt(agentIndex);
                Grid[newX, newY].Add(agent);
                agent.MoveAgentInSpace(newX, newY);
            }
        }
    }

    public void AddAgent(Agent agent, int x, int y)
    {
        Grid[x, y].Add(agent);
        agent.MoveAgentInSpace(x, y);
    }

    public List<Agent> GetAgents()
    {
        var allAgents = new List<Agent>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                allAgents.AddRange(Grid[x, y]);
            }
        }

        return allAgents;
    }
}