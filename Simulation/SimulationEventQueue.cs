namespace Simulation;

public class SimulationEventQueue
{
    private readonly List<SimulationEvent> _events;

    public SimulationEventQueue()
    {
        _events = new List<SimulationEvent>();
    }
    
    public int Count => _events.Count;
    public float CurrentTick => _events.First().Tick;

    public void Enqueue(SimulationEvent simulationEvent)
    {
        var index = _events.BinarySearch(simulationEvent, Comparer<SimulationEvent>.Create((event1, event2) => event1.Tick.CompareTo(event2.Tick)));
        if (index < 0)
            index = ~index;
        
        _events.Insert(index, simulationEvent);
    }

    public SimulationEvent? Dequeue()
    {
        if (_events.Count <= 0)
            return null;
        
        var simulationEvent = _events[0];
        _events.RemoveAt(0);
        return simulationEvent;
    }
}