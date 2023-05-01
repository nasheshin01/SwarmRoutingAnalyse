using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Simulation;

namespace SimulationUI;

public partial class ConditionsEditorWindow : Window
{
    private readonly ObservableCollection<ConditionControl> _conditionControls;
    private readonly AgentTypeForRules _agentType;
    
    
    public ConditionsEditorWindow(AgentTypeForRules agentType, List<RuleCondition> conditions) 
    {
        InitializeComponent();

        _conditionControls = new ObservableCollection<ConditionControl>();
        if (conditions.Count == 0)
        {
            var firstConditionControl = new ConditionControl(agentType);
            _conditionControls.Add(firstConditionControl);
        }
        else
        {
            foreach (var condition in conditions)
            {
                var conditionControl = new ConditionControl(agentType, condition);
                _conditionControls.Add(conditionControl);
            }
        }
        ConditionsList.ItemsSource = _conditionControls;
        
        _agentType = agentType;
    }

    public List<RuleCondition> GetResultConditions()
    {
        return _conditionControls.Select(conditionControl => conditionControl.GetResultCondition()).ToList();
    }

    private void OnAddConditionButtonClicked(object sender, RoutedEventArgs e)
    {
        var conditionControl = new ConditionControl(_agentType);
        _conditionControls.Add(conditionControl);
    }
}