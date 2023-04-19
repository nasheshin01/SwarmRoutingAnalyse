namespace Simulation.Agents;

public class Scout : DataAgent
{
    private readonly float _maxDroneDistance;
    private readonly float _energyLimit;


    public Scout(int id, Drone currentDrone, int dataSize, float instantiateTime, float energyLimit,
        float maxDroneDistance) : base(id, currentDrone, dataSize, instantiateTime)
    {
        _energyLimit = energyLimit;
        _maxDroneDistance = maxDroneDistance;

        Energy = energyLimit;
        ScoutState = ScoutState.Scouting;
        Path = new List<Drone>() {CurrentDrone};
    }

    public float Energy { get; set; }
    public ScoutState ScoutState { get; set; }
    public List<Drone> Path { get; set; }

    public override void DoAction(List<Drone> drones)
    {
        if (ScoutState == ScoutState.Scouting && CurrentDrone is EndDrone)
            ScoutState = ScoutState.GoingToStart;
        if (ScoutState == ScoutState.GoingToStart && CurrentDrone is StartDrone)
            ScoutState = ScoutState.ScoutingEnded;
        
        if (ScoutState == ScoutState.Scouting)
        {
            var closeDrones = drones.Where(drone => !(Utils.GetDroneDistance(CurrentDrone, drone) > _maxDroneDistance))
                .ToList();
            closeDrones = closeDrones.Where(drone => !Path.Contains(drone)).ToList();
            if (closeDrones.Count < 0)
            {
                DestroyAgent();
                return;
            }

            var randomDrone = closeDrones[new Random().Next(0, closeDrones.Count)];
            LoadToDrone(randomDrone);
            Path.Add(randomDrone);
            Energy--;
        }
        else if (ScoutState == ScoutState.GoingToStart)
        {
            Drone? nextDrone = null;
            for (var i = 0; i < Path.Count; i++)
            {
                if (Path[i] != CurrentDrone)
                    continue;

                nextDrone = Path[i - 1];
                break;
            }

            if (nextDrone == null)
            {
                DestroyAgent();
                return;
            }

            if (Utils.GetDroneDistance(CurrentDrone, nextDrone) > _maxDroneDistance)
            {
                DestroyAgent();
                return;
            }
            
            LoadToDrone(nextDrone);
            Energy--;
        }
        else if (ScoutState == ScoutState.ScoutingEnded)
        {
            //Give data about found path
        }
    }
}