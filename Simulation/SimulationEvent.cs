namespace Simulation;

internal class SimulationEvent
{
    private int _tick;
    private Action _action;

    public SimulationEvent(int tick, Action action)
    {
        _tick = tick;
        _action = action;
    }
}