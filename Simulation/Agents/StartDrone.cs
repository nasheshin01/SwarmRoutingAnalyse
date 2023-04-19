namespace Simulation.Agents;

public class StartDrone : Drone
{
    private readonly List<List<Drone>> _suitablePaths;
    private const float LoadingSpeed = 15f;
    
    public StartDrone(int id, int x, int y) : base(id, x, y)
    {
        _suitablePaths = new List<List<Drone>>();
    }

    public override void DoAction(GridSpace gridSpace)
    {
        base.DoAction(gridSpace);
    }

    public bool TryAddNewPath(List<Drone> path)
    {
        var intersectedPaths = _suitablePaths.Where(sp => IsPathsIntersects(path, sp)).ToList();
        if (!intersectedPaths.Any())
        {
            _suitablePaths.Add(path);
            return true;
        }
        
        var sumSpeedOfIntersectedPaths = intersectedPaths.Sum(sp => Utils.GetPathLoadingSpeed(sp, LoadingSpeed));
        if (sumSpeedOfIntersectedPaths > Utils.GetPathLoadingSpeed(path, LoadingSpeed))
            return false;

        foreach (var intersectedPath in intersectedPaths)
        {
            _suitablePaths.Remove(intersectedPath);
        }
        _suitablePaths.Add(path);
        return true;
    }

    public bool IsSuitablePathExists => _suitablePaths.Count > 0;
    public List<List<Drone>> SuitablePaths => _suitablePaths;

    private bool IsPathsIntersects(List<Drone> path1, List<Drone> path2)
    {
        foreach (var droneFromPath1 in path1)   
            foreach (var droneFromPath2 in path2)
                if (droneFromPath1 == droneFromPath2)
                    return true;

        return false;
    }
}