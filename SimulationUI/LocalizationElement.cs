namespace SimulationUI;

public class LocalizationElement
{
    public LocalizationElement(string name, string localizedName)
    {
        Name = name;
        LocalizedName = localizedName;
    }
    
    public string Name { get; }
    public string LocalizedName { get; }
}