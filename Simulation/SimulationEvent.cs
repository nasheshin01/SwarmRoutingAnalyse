namespace Simulation;

public class SimulationEvent
{
    private readonly Action _action;

    public SimulationEvent(float tick, Action action)
    {
        _action = action;
        
        Tick = tick;
    }

    public float Tick { get; }

    public void Execute()
    {
        _action();
    }
}