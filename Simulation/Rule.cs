namespace Simulation;

public class Rule
{
    public Rule(string name, AgentTypeForRules agentType, List<RuleCondition> conditions, string action)
    {
        Name = name;
        AgentType = agentType;
        Conditions = conditions;
        Action = action;
    }

    public string Name { get; set; }
    public AgentTypeForRules AgentType { get; set; }
    public List<RuleCondition> Conditions { get; set; }
    public string Action { get; set; }
}