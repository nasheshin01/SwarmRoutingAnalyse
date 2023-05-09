using System.Collections.Generic;
using Simulation;

namespace SimulationUI;

public class ConditionTemplate
{
    public ConditionTemplate(LocalizationElement variable, EquationType equationsType, List<LocalizationElement> values)
    {
        Variable = variable;
        EquationType = equationsType;
        Equations = equationsType == EquationType.Int
            ? new List<LocalizationElement> { Localizations.ConditionEquationEquals, Localizations.ConditionEquationNotEquals, Localizations.ConditionEquationMore, Localizations.ConditionEquationLess}
            : new List<LocalizationElement> { Localizations.ConditionEquationEquals, Localizations.ConditionEquationNotEquals};
        Values = values;
    }
    
    public LocalizationElement Variable { get; set; }
    public EquationType EquationType { get; }
    public List<LocalizationElement> Equations { get; set; }
    public List<LocalizationElement> Values { get; set; }
}