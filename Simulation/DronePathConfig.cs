using Simulation.Agents;

namespace Simulation;

public class DronePathConfig
{
    public DronePathConfig(Drone startDrone, Drone endDrone)
    {
        StartDrone = startDrone;
        EndDrone = endDrone;
    }

    public Drone StartDrone { get; set; }
    public Drone EndDrone { get; set; }
}