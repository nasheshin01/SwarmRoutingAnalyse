namespace Simulation;

public class Package
{
    private readonly Data _parentData;
    private readonly int _id;
    
    public Package(Data parentData, int id)
    {
        _parentData = parentData;
        _id = id;
    }
    
    public int Id => _id;
    public Data ParentData => _parentData;
}

public class Data
{
    private readonly int _id;
    private readonly List<Package> _packages;
    
    public Data(int id, int dataSize)
    {
        _id = id;
        _packages = new List<Package>();
        for (var i = 0; i < dataSize; i++)
        {
            _packages.Add(new Package(this, i));
        }
    }

    public int Id => _id;
    public List<Package> Packages => _packages;
    
}