using System.Data;
using System.Drawing;

namespace Simulation.Agents;

public class Drone : Agent
{
    public Drone(int id, int x, int y, List<Rule> rules) : base(id, rules)
    {
        X = x;
        Y = y;
        Energy = 100;
        HibernationStatus = HibernationStatus.NoHibernate;

        DataLoadingProcesses = new List<DataLoadingProcess>();
    }

    public virtual void DoAction(Size boundaries)
    {
        foreach (var rule in Rules)
        {
            if (rule.AgentType is not AgentTypeForRules.Drone)
                continue;
            
            var droneType = typeof(Drone);
            var variables = droneType.GetProperties().Select(p => p.Name).ToList();
            var isConditionsAreTrue = CheckConditions(rule.Conditions, variables, droneType);
            if (!isConditionsAreTrue)
                continue;

            if (rule.Action == "RandomMove")
                RandomMove(boundaries);
            else if (rule.Action == "EnableHibernation")
                EnableHibernation();
            else if (rule.Action == "DisableHibernation")
                DisableHibernation();
            else
                throw new Exception("Unknown action");
        }
        
        Energy += HibernationStatus == HibernationStatus.Hibernate ? 1 : -1;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Energy { get; set; }
    public HibernationStatus HibernationStatus { get; set; }
    public List<DataLoadingProcess> DataLoadingProcesses { get; set; }

    private void EnableHibernation()
    {
        HibernationStatus = HibernationStatus.Hibernate;
    }

    private void DisableHibernation()
    {
        HibernationStatus = HibernationStatus.NoHibernate;
    }
    
    private void RandomMove(Size boundaries)
    {
        var dx = new Random().Next(-1, 2);
        var dy = new Random().Next(-1, 2);
        X += dx;
        Y += dy;

        if (X < 0)
            X = 0;
        if (X >= boundaries.Width)
            X = boundaries.Width - 1;
        if (Y < 0)
            Y = 0;
        if (Y >= boundaries.Height)
            Y = boundaries.Height - 1;
    }
}