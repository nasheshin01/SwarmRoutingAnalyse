using System.Drawing;

namespace Simulation.Agents;

public class StartDrone : Drone
{
    private readonly List<Path> _suitablePaths;
    private readonly float _constLoadingSpeed;
    private int _currentPathId;
    
    public StartDrone(int id, int x, int y, float constLoadingSpeed) : base(id, x, y)
    {
        _suitablePaths = new List<Path>();

        _constLoadingSpeed = constLoadingSpeed;
        SuitablePathsUpdated = false;
    }

    public bool TryAddNewPath(List<Drone> drones, float loadingSpeed)
    {
        if (_suitablePaths.Any(sp => sp.Drones.Count == 2) && drones.Count == 2)
        {
            return false;
        }
        
        var path = new Path(_currentPathId, drones, loadingSpeed);
        _currentPathId++;
        var intersectedPaths = _suitablePaths.Where(sp => sp.IsPathIntersects(path)).ToList();
        if (!intersectedPaths.Any())
        {
            _suitablePaths.Add(path);
            SuitablePathsUpdated = true;
            return true;
        }
        
        var sumSpeedOfIntersectedPaths = intersectedPaths.Sum(sp => sp.LoadingSpeed);
        if (sumSpeedOfIntersectedPaths > path.LoadingSpeed)
            return false;

        foreach (var intersectedPath in intersectedPaths)
        {
            _suitablePaths.Remove(intersectedPath);
        }
        _suitablePaths.Add(path);
        SuitablePathsUpdated = true;
        return true;
    }

    public void UpdateSuitablePath(int id, float loadingSpeed)
    {
        var path = _suitablePaths.FirstOrDefault(sp => sp.Id == id);
        if (path == null)
            return;

        path.LoadingSpeed = loadingSpeed;
        SuitablePathsUpdated = true;
    }

    public void RemoveSuitablePath(Path path)
    {
        _suitablePaths.Remove(path);
    }

    public bool IsSuitablePathExists => _suitablePaths.Count > 0;
    public List<Path> SuitablePaths => _suitablePaths;
    public bool SuitablePathsUpdated { get; set; }
}