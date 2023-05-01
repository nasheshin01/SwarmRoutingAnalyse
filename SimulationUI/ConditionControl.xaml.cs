using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Simulation;

namespace SimulationUI;

public partial class ConditionControl
{
    private List<ConditionTemplate> _conditionTemplates;
    private ObservableCollection<string> _variableBoxItems;
    private ObservableCollection<Equation> _equationBoxItems;
    private ObservableCollection<string> _valueBoxItems;

    public ConditionControl(AgentTypeForRules agentType)
    {
        InitializeComponent();

        InitializeTemplates(agentType);
        if (_conditionTemplates == null)
            throw new ArgumentNullException(nameof(_conditionTemplates));

        _variableBoxItems = new ObservableCollection<string>(_conditionTemplates.Select(cp => cp.Variable));
        _equationBoxItems = new ObservableCollection<Equation>();
        _valueBoxItems = new ObservableCollection<string>();

        VariableBox.ItemsSource = _variableBoxItems;
        VariableBox.SelectedIndex = 0;
        EquationBox.ItemsSource = _equationBoxItems;
        ValueBox.ItemsSource = _valueBoxItems;
    }
    
    public ConditionControl(AgentTypeForRules agentType, RuleCondition condition)
    {
        InitializeComponent();

        InitializeTemplates(agentType);
        if (_conditionTemplates == null)
            throw new ArgumentNullException(nameof(_conditionTemplates));

        _variableBoxItems = new ObservableCollection<string>(_conditionTemplates.Select(cp => cp.Variable));
        _equationBoxItems = new ObservableCollection<Equation>();
        _valueBoxItems = new ObservableCollection<string>();

        VariableBox.ItemsSource = _variableBoxItems;
        VariableBox.SelectedIndex = 0;
        EquationBox.ItemsSource = _equationBoxItems;
        ValueBox.ItemsSource = _valueBoxItems;

        VariableBox.SelectedItem = condition.Variable;
        EquationBox.SelectedItem = condition.Equation;

        if (condition.EquationType == EquationType.Enum)
            ValueBox.SelectedIndex = condition.Value;
        else
            ValueBox.Text = condition.Value.ToString();
    }

    private void InitializeTemplates(AgentTypeForRules agentType)
    {
        switch (agentType)
        {
            case AgentTypeForRules.Drone:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new("X", EquationType.Int, new List<string>()),
                    new("Y", EquationType.Int, new List<string>()),
                    new("Energy", EquationType.Int, new List<string>()),
                    new("HibernationStatus", EquationType.Enum, new List<string> { "Hibernate", "NoHibernate" })
                };
                break;
            case AgentTypeForRules.Scout:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new("Energy", EquationType.Int, new List<string>()),
                    new("ScoutState",  EquationType.Enum, new List<string> { "Scouting", "GoingToStart", "ScoutingEnded" }),
                    new("WayBackLoadingTime", EquationType.Int, new List<string>())
                };
                break;
            case AgentTypeForRules.Worker:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new("WorkerState",  EquationType.Enum, new List<string> { "Sending", "GoingToStart", "SendingEnded" }),
                    new("WayBackLoadingTime", EquationType.Int, new List<string>())
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(agentType), agentType, null);
        }
    }

    public RuleCondition GetResultCondition()
    {
        var variable = (string)VariableBox.SelectedItem;
        var equation = (Equation)EquationBox.SelectedItem;
        var equationType = EquationType.Int;
        
        var isNum = int.TryParse(ValueBox.Text, out var value);
        if (isNum)
            return new RuleCondition(variable, equationType, equation, value);
        
        value = ValueBox.SelectedIndex;
        equationType = EquationType.Enum;

        return new RuleCondition(variable, equationType, equation, value);
    }

    private void VariableSelectedChanged(object sender, SelectionChangedEventArgs e)
    {
        var conditionTemplate =
            _conditionTemplates.FirstOrDefault(cp => cp.Variable == (string)VariableBox.SelectedItem);
        if (conditionTemplate == null)
            throw new Exception("Condition template not found");
        
        _equationBoxItems.Clear();
        foreach (var equation in conditionTemplate.Equations)
        {
            _equationBoxItems.Add(equation);
        }
        EquationBox.SelectedIndex = 0;

        _valueBoxItems.Clear();
        foreach (var value in conditionTemplate.Values)
        {
            _valueBoxItems.Add(value);
        }

        if (_valueBoxItems.Count == 0)
            ValueBox.Text = "0";
        else
            ValueBox.SelectedIndex = 0;
    }
}