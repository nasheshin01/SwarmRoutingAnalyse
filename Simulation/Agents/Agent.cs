namespace Simulation.Agents;

public abstract class Agent
{
    protected Agent(int id)
    {
        Id = id;
        IsDestroyed = false;
    }
    
    protected void DestroyAgent() => IsDestroyed = true;

    public virtual void MoveAgentInSpace(int x, int y)
    {
    }

    public int Id { get; set; }
    public bool IsDestroyed { get; set; }
}