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
    
    public int Id { get; set; }
}