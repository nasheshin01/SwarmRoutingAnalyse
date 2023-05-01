using System.Data;

namespace Simulation.Agents;

public abstract class Agent
{
    protected Agent(int id, List<Rule> rules)
    {
        Id = id;
        IsDestroyed = false;
        Rules = rules;
    }
    
    protected void DestroyAgent() => IsDestroyed = true;
    
    protected bool CheckConditions(List<RuleCondition> conditions, List<string> variables, Type type)
    {
        var isConditionsAreTrue = true;
        foreach (var condition in conditions)
        {
            var expr = $"{condition.Variable} {condition.Equation.ToStringEquation()} {condition.Value}";
            
            foreach (var variable in variables)
            {
                var property = type.GetProperty(variable);
                if (property == null) 
                    continue;
                
                var value = (int)(property.GetValue(this) ?? 0);
                expr = expr.Replace(variable, value.ToString());
            }
            
            isConditionsAreTrue = isConditionsAreTrue && (bool)new DataTable().Compute(expr, "");
        }

        return isConditionsAreTrue;
    }

    public int Id { get; set; }
    public bool IsDestroyed { get; set; }
    
    public List<Rule> Rules { get; set; }
    
    
}