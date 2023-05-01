using System.ComponentModel;

namespace Simulation;

public enum AgentTypeForRules
{
    Drone = 0,
    Scout = 1,
    Worker = 2,
    StartDrone = 3,
    EndDrone = 4,
    DroneMoveLogic = 5,
    ScoutDroneChooseLogic = 6
}