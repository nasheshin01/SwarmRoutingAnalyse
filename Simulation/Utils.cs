using Simulation.Agents;

namespace Simulation;

public static class Utils
{
    public static float GetDroneDistance(Drone drone1, Drone drone2)
    {
        return (float) Math.Sqrt(Math.Pow(drone1.X - drone2.X, 2) + Math.Pow(drone1.Y - drone2.Y, 2));
    }
}