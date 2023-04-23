using Simulation.Agents;

namespace Simulation;

public class Path
{
    public Path(int id, List<Drone> drones, float loadingSpeed)
    {
        Id = id;
        Drones = drones;
        LoadingSpeed = loadingSpeed;
    }

    public int Id { get; set; }
    public List<Drone> Drones { get; set; }
    public float LoadingSpeed { get; set; }

    public bool IsPathIntersects(Path other)
    {
        foreach (var droneFromPath1 in Drones)   
            foreach (var droneFromPath2 in other.Drones)
                if (droneFromPath1 == droneFromPath2 && droneFromPath1 is not StartDrone && droneFromPath1 is not EndDrone)
                    return true;

        return false;
    }
}