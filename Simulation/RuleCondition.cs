namespace Simulation;

public class RuleCondition
{
    public RuleCondition(string variable, EquationType equationType, Equation equation, int value)
    {
        Variable = variable;
        EquationType = equationType;
        Equation = equation;
        Value = value;
    }
    public string Variable { get; set; }
    public EquationType EquationType { get; set; }
    public Equation Equation { get; set; }
    public int Value { get; set; }
}