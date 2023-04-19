namespace Simulation.Agents;

public abstract class Agent
{
    protected Agent(int id)
    {
        Id = id;
    }
    
    protected void DestroyAgent()
    {
        throw new NotImplementedException();
    }

    public virtual void MoveAgentInSpace(int x, int y)
    {
    }

    public int Id { get; set; }
}