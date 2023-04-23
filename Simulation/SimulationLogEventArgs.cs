namespace Simulation;

public class SimulationLogEventArgs : EventArgs
{
    private readonly string _logMessage;
    private readonly float _tick;
    private readonly LogType _logType;
    
    public SimulationLogEventArgs(string logMessage, float tick, LogType logType)
    {
        _logMessage = logMessage;
        _tick = tick;
        _logType = logType;
    }

    public string LogMessage => _logMessage;
    public float Tick => _tick;
    public LogType LogType => _logType;
}