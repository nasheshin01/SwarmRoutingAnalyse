namespace Simulation;

public class SimulationConfig
{
    public int GridWidth = 100;
    public int GridHeight = 100;
    public int DroneCount = 100;
    public int ScoutCount = 10;
    public int WorkerCount = 1;
    public int DataCount = 8;
    public float DataGeneratePeriod = 100f;
    public int DataSize = 8;
    public int PackageSize = 8;

    public MapConfig MapConfig;
}