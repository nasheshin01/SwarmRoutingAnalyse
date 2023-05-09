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
    private ObservableCollection<LocalizationElement> _variableBoxItems;
    private ObservableCollection<LocalizationElement> _equationBoxItems;
    private ObservableCollection<LocalizationElement> _valueBoxItems;

    public ConditionControl(AgentTypeForRules agentType)
    {
        InitializeComponent();

        InitializeTemplates(agentType);
        if (_conditionTemplates == null)
            throw new ArgumentNullException(nameof(_conditionTemplates));

        _variableBoxItems = new ObservableCollection<LocalizationElement>(_conditionTemplates.Select(cp => cp.Variable));
        _equationBoxItems = new ObservableCollection<LocalizationElement>();
        _valueBoxItems = new ObservableCollection<LocalizationElement>();

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

        _variableBoxItems = new ObservableCollection<LocalizationElement>(_conditionTemplates.Select(cp => cp.Variable));
        _equationBoxItems = new ObservableCollection<LocalizationElement>();
        _valueBoxItems = new ObservableCollection<LocalizationElement>();

        VariableBox.ItemsSource = _variableBoxItems;
        VariableBox.SelectedIndex = 0;
        EquationBox.ItemsSource = _equationBoxItems;
        ValueBox.ItemsSource = _valueBoxItems;

        VariableBox.Text = _variableBoxItems.First(i => i.Name == condition.Variable).LocalizedName;
        EquationBox.Text = _equationBoxItems.First(i => i.Name == condition.Equation.ToString()).LocalizedName;

        ValueBox.Text = condition.EquationType != EquationType.Int
            ? _valueBoxItems.First(i => i.Name == condition.Value).LocalizedName
            : condition.Value;
    }

    private void InitializeTemplates(AgentTypeForRules agentType)
    {
        switch (agentType)
        {
            case AgentTypeForRules.Drone:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new(Localizations.ConditionVariableX, EquationType.Int, new List<LocalizationElement>()),
                    new(Localizations.ConditionVariableY, EquationType.Int, new List<LocalizationElement>()),
                    new(Localizations.ConditionVariableEnergy, EquationType.Int, new List<LocalizationElement>()),
                    new(Localizations.ConditionVariableHibernationStatus, EquationType.Enum, new List<LocalizationElement> { Localizations.ConditionValueHibernate, Localizations.ConditionValueNoHibernate })
                };
                break;
            case AgentTypeForRules.Scout:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new(Localizations.ConditionVariableEnergy, EquationType.Int, new List<LocalizationElement>()),
                    new(Localizations.ConditionVariableScoutState,  EquationType.Enum, new List<LocalizationElement> { Localizations.ConditionValueScouting, Localizations.ConditionValueGoingToStart, Localizations.ConditionValueScoutingEnded }),
                    new(Localizations.ConditionVariableIsNextDroneOutOfRadius, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsNextDroneInLoadingState, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsCurrentDroneSource, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsCurrentDroneDestination, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsNextDroneChoosed, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                };
                break;
            case AgentTypeForRules.Worker:
                _conditionTemplates = new List<ConditionTemplate>
                {
                    new(Localizations.ConditionVariableWorkerState,  EquationType.Enum, new List<LocalizationElement> { Localizations.ConditionValueSending, Localizations.ConditionValueGoingToStart, Localizations.ConditionValueSendingEnded }),
                    new(Localizations.ConditionVariableIsNextDroneOutOfRadius, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsNextDroneBusy, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsCurrentDroneSource, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsCurrentDroneDestination, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                    new(Localizations.ConditionVariableIsNextDroneChosen, EquationType.Bool, new List<LocalizationElement> {Localizations.ConditionValueTrue, Localizations.ConditionValueFalse}),
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(agentType), agentType, null);
        }
    }

    public RuleCondition GetResultCondition()
    {
        var variable = ((LocalizationElement)VariableBox.SelectedItem).Name;
        var equation = Enum.Parse<Equation>(((LocalizationElement)EquationBox.SelectedItem).Name);

        EquationType equationType;
        if (int.TryParse(ValueBox.Text, out _))
        {
            equationType = EquationType.Int;
            return new RuleCondition(variable, equationType, equation, ValueBox.Text);
        }

        equationType = ((LocalizationElement)ValueBox.SelectedItem).Name is "True" or "False"
                ? EquationType.Bool
                : EquationType.Enum;
        return new RuleCondition(variable, equationType, equation, ((LocalizationElement)ValueBox.SelectedItem).Name);
    }

    private void VariableSelectedChanged(object sender, SelectionChangedEventArgs e)
    {
        var conditionTemplate =
            _conditionTemplates.FirstOrDefault(cp => cp.Variable == (LocalizationElement)VariableBox.SelectedItem);
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

        ValueBox.IsEditable = conditionTemplate.EquationType == EquationType.Int;
    }   
}