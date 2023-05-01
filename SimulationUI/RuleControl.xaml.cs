using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Simulation;

namespace SimulationUI;

public partial class RuleControl : UserControl
{
    private readonly Dictionary<AgentTypeForRules, List<string>> _actionOptions = new()
    {
        { AgentTypeForRules.Drone, new List<string> {"EnableHibernation", "DisableHibernation", "RandomMove"} },
        { AgentTypeForRules.Scout, new List<string> {"DestroyYourself"} },
        { AgentTypeForRules.Worker, new List<string> {"DestroyYourself"} },
    }; 

    private ObservableCollection<string> _actionItems;

    public RuleControl(string ruleName)
    {
        InitializeComponent();

        RuleName = ruleName;
        Conditions = new List<RuleCondition>();

        InitializeLists();
    }
    
    public RuleControl(Rule rule)
    {
        InitializeComponent();

        RuleNameBox.Text = rule.Name;
        
        Conditions = rule.Conditions;

        InitializeLists();

        AgentTypeList.SelectedItem = rule.AgentType;
        ActionsList.SelectedItem = rule.Action;
    }
    
    public string RuleName { get; set; }
    public List<RuleCondition> Conditions { get; set; }

    public Rule GetResultRule()
    {
        var rule = new Rule(RuleName, (AgentTypeForRules)AgentTypeList.SelectedItem, Conditions,
            (string)ActionsList.SelectedItem);
        return rule;
    }

    private void InitializeLists()
    {
        _actionItems = new ObservableCollection<string>();
        ActionsList.ItemsSource = _actionItems;
        AgentTypeList.ItemsSource = new ObservableCollection<AgentTypeForRules>(Enum.GetValues<AgentTypeForRules>());
        AgentTypeList.SelectedIndex = 0;
    }
    
    private void OnNameChanged(object sender, TextChangedEventArgs e)
    {
        RuleName = ((TextBox)sender).Text;
    }

    private void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
    {
        var agentType = (AgentTypeForRules)AgentTypeList.SelectedItem;
        
        _actionItems.Clear();
        foreach (var actionOption in _actionOptions[agentType])
        {
            _actionItems.Add(actionOption);
        }

        ActionsList.SelectedIndex = 0;
    }

    private void OnEditConditionsButtonClicked(object sender, RoutedEventArgs e)
    {
        var agentType = (AgentTypeForRules)AgentTypeList.SelectedItem;
        var conditionsEditorWindow = new ConditionsEditorWindow(agentType, Conditions);
        
        var result = conditionsEditorWindow.ShowDialog();

        Conditions = conditionsEditorWindow.GetResultConditions();
    }

    private void OnActionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}