namespace Simulation;

public class SimulationConfig
{
    public int ScoutCount = 10;
    public int DataCount = 8;
    public float DataGeneratePeriod = 100f;
    public int DataSize = 8;
    public int PackageSize = 8;
    public int ScoutSize = 1;
    public float ScoutEnergyLimit = 100;
    public float MaxDroneDistance = 20;
    public float DroneMovePeriod = 10;
    public float EndSimulationTick = 2000;
    public float ConstLoadingSpeed = 20f;
    

    public MapConfig MapConfig;
    public List<Rule> Rules = new();
}