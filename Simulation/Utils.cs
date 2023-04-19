using Simulation.Agents;

namespace Simulation;

public static class Utils
{
    public static float GetDroneDistance(Drone drone1, Drone drone2)
    {
        return (float) Math.Sqrt(Math.Pow(drone1.X - drone2.X, 2) + Math.Pow(drone1.Y - drone2.Y, 2));
    }

    public static float GetLoadingSpeed(Drone drone1, Drone drone2, float constLoadingSpeed)
    {
        var droneDistance = GetDroneDistance(drone1, drone2);
        return constLoadingSpeed / (droneDistance * droneDistance);
    }
    
    public static float GetPathLoadingSpeed(List<Drone> path, float constLoadingSpeed)
    {
        var pathLoadingSpeed = 0f;
        for (var i = 0; i < path.Count - 1; i++)
        {
            pathLoadingSpeed += GetLoadingSpeed(path[i], path[i + 1], constLoadingSpeed);
        }

        return pathLoadingSpeed;
    }
}