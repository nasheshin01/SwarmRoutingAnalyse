using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Simulation;

namespace SimulationUI;

public partial class RuleEditorWindow
{
    private readonly Dictionary<AgentTypeForRules, ObservableCollection<RuleControl>> _ruleControls;

    public RuleEditorWindow(List<Rule> rules)
    {
        InitializeComponent();

        _ruleControls = new Dictionary<AgentTypeForRules, ObservableCollection<RuleControl>>();
        foreach (var agentType in Enum.GetValues<AgentTypeForRules>())
        {
            _ruleControls.Add(agentType, new ObservableCollection<RuleControl>());
        }
        
        foreach (var rule in rules)
        {
            var ruleControl = new RuleControl(rule);
            _ruleControls[rule.AgentType].Add(ruleControl);
        }

        DroneRulesList.ItemsSource = _ruleControls[AgentTypeForRules.Drone];
        ScoutRulesList.ItemsSource = _ruleControls[AgentTypeForRules.Scout];
        WorkerRulesList.ItemsSource = _ruleControls[AgentTypeForRules.Worker];
    }

    public List<Rule> GetResultRules()
    {
        var resultRules = new List<Rule>();
        foreach (var agentType in _ruleControls.Keys)
        {
            resultRules.AddRange(_ruleControls[agentType].Select(rc => rc.GetResultRule()).ToList());
        }
        
        return resultRules;
    }

    private AgentTypeForRules GetChosenAgentTypeByTab()
    {
        return (string)((TabItem)RuleTabControl.SelectedItem).Header switch
        {
            "Дрон" => AgentTypeForRules.Drone,
            "Разведчик" => AgentTypeForRules.Scout,
            "Работник" => AgentTypeForRules.Worker,
            _ => AgentTypeForRules.Drone
        };
    }
    
    private void OnAddRuleButtonClicked(object sender, RoutedEventArgs e)
    {
        var agentType = GetChosenAgentTypeByTab();
        _ruleControls[agentType].Add(new RuleControl("Правило", agentType));
    }

    private void OnDeleteRuleButtonClicked(object sender, RoutedEventArgs e)
    {
        if ((string)((TabItem)RuleTabControl.SelectedItem).Header == "Дрон")
            _ruleControls[AgentTypeForRules.Drone].RemoveAt(DroneRulesList.SelectedIndex);
        else if ((string)((TabItem)RuleTabControl.SelectedItem).Header == "Разведчик")
            _ruleControls[AgentTypeForRules.Scout].RemoveAt(ScoutRulesList.SelectedIndex);
        else if ((string)((TabItem)RuleTabControl.SelectedItem).Header == "Работник")
            _ruleControls[AgentTypeForRules.Worker].RemoveAt(WorkerRulesList.SelectedIndex);
    }
}