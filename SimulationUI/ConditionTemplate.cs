using System.Collections.Generic;
using Simulation;

namespace SimulationUI;

public class ConditionTemplate
{
    public ConditionTemplate(string variable, EquationType equationsType, List<string> values)
    {
        Variable = variable;
        Equations = equationsType == EquationType.Int
            ? new List<Equation> { Equation.Equal, Equation.NotEqual, Equation.More, Equation.Less}
            : new List<Equation> { Equation.Equal, Equation.NotEqual};
        Values = values;
    }
    
    public string Variable { get; set; }
    public List<Equation> Equations { get; set; }
    public List<string> Values { get; set; }
}