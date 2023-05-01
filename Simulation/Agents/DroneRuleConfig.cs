using System.Data;

namespace Simulation.Agents;

public class DroneRule
{
    public DroneRule(string conditionTemplate, string actionTemplate)
    {
        ConditionTemplate = conditionTemplate;
        ActionTemplate = actionTemplate;

        // Compile the condition and action expressions from the string templates
        Condition = d => CompileExpression<bool>(d, ConditionTemplate);
        Action = d => CompileExpression<object>(d, ActionTemplate);
    }
    
    public Func<Drone, bool> Condition { get; set; }
    public Action<Drone> Action { get; set; }
    
    // String template for the condition expression
    public string ConditionTemplate { get; set; }

    // String template for the action expression
    public string ActionTemplate { get; set; }
    
    private T CompileExpression<T>(Drone drone, string template)
    {
        var variables = new[] { "X", "Y" }; // define the variables that can be used in the expression
        var expr = template;
        
        // Replace the variables with their corresponding property values using reflection
        foreach (var variable in variables)
        {
            var property = typeof(Drone).GetProperty(variable);
            if (property != null)
            {
                var value = property.GetValue(drone);
                expr = expr.Replace(variable, value.ToString());
            }
        }
        
        // Compile and evaluate the expression
        return (T)new DataTable().Compute(expr, "");
    }
}

public class DroneRuleConfig
{
    public DroneRuleConfig()
    {
        Rules = new List<DroneRule>();
    }
    
    public List<DroneRule> Rules { get; set; }
}