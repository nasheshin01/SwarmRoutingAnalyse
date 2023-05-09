using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Simulation;

namespace SimulationUI;

public partial class RuleControl : UserControl
{
    private readonly Dictionary<AgentTypeForRules, List<string>> _actionOptionsToConfig = new()
    {
        { AgentTypeForRules.Drone, new List<string> { "EnableHibernation", "DisableHibernation", "RandomMove" } },
        {
            AgentTypeForRules.Scout, new List<string>
            {
                "AddBestPath", "LoadToNextDrone", "ChangeStateToGoingToStart", "ChangeStateToScout",
                "ChangeStateToScoutEnd", "ChooseNextDroneToGoBack", "ChooseNextRandomDrone", "DestroyAgent"
            }
        },
        {
            AgentTypeForRules.Worker, new List<string>
            {
                "UpdateBestPath", "LoadToNextDrone", "ChangeStateToGoingToStart", "ChangeStateToSend", 
                "ChangeStateToSendEnd", "ChooseNextDroneForward", "ChooseNextDroneBack", "SetPackageLost", 
                "SetPackageReceived", "DestroyAgent", "DoNothing"
            }
        },
    };

    private readonly Dictionary<AgentTypeForRules, List<string>> _actionOptions = new()
    {
        {
            AgentTypeForRules.Drone,
            new List<string> { "Включить гибернацию", "Выключить гибернацию", "Случайное движение" }
        },
        {
            AgentTypeForRules.Scout, new List<string>
            {
                "Добавить лучший путь", "Начать загрузку разведчика в выбранного дрона",
                "Поменять статус на \"Возвращение назад\"", "Поменять статус на \"Разведка\"",
                "Поменять статус на \"Разведка закончена\"", "Выбрать следующего дрона для пути назад",
                "Выбрать следующего случайного дрона", "Уничтожить разведчика"
            }
        },
        {
            AgentTypeForRules.Worker, new List<string>
            {
                "Обновить лучший путь", "Начать загрузку работника в выбранного дрона", 
                "Поменять статус на \"Возвращение назад\"", "Поменять статус на \"Отправка пакета\"", 
                "Поменять статус на \"Отправка закончена\"", "Выбрать следующего дрона для пути вперед",
                "Выбрать следующего дрона для пути назад", "Назанчить пакет данных потерянным", 
                "Назанчить пакет данных полученным", "Уничтожить работника", "Закончить действие"
            }
        },
    }; 

    private ObservableCollection<string> _actionItems;
    private AgentTypeForRules _agentType;

    public RuleControl(string ruleName, AgentTypeForRules agentType)
    {
        InitializeComponent();

        RuleName = ruleName;
        RuleNameBox.Text = RuleName;
        Conditions = new List<RuleCondition>();
        _agentType = agentType;

        InitializeLists();
    }
    
    public RuleControl(Rule rule)
    {
        InitializeComponent();

        RuleNameBox.Text = rule.Name;
        
        Conditions = rule.Conditions;
        _agentType = rule.AgentType;

        InitializeLists();
        
        ActionsList.SelectedItem = _actionOptions[_agentType][_actionOptionsToConfig[_agentType].IndexOf(rule.Action)];
    }
    
    public string RuleName { get; set; }
    public List<RuleCondition> Conditions { get; set; }

    public Rule GetResultRule()
    {
        var rule = new Rule(RuleName, _agentType, Conditions,
            _actionOptionsToConfig[_agentType][ActionsList.SelectedIndex]);
        return rule;
    }

    private void InitializeLists()
    {
        _actionItems = new ObservableCollection<string>();
        ActionsList.ItemsSource = _actionItems;

        _actionItems.Clear();
        foreach (var actionOption in _actionOptions[_agentType])
        {
            _actionItems.Add(actionOption);
        }

        ActionsList.SelectedIndex = 0;
    }
    
    private void OnNameChanged(object sender, TextChangedEventArgs e)
    {
        RuleName = ((TextBox)sender).Text;
    }

    private void OnEditConditionsButtonClicked(object sender, RoutedEventArgs e)
    {
        var conditionsEditorWindow = new ConditionsEditorWindow(_agentType, Conditions);
        
        var result = conditionsEditorWindow.ShowDialog();

        Conditions = conditionsEditorWindow.GetResultConditions();
    }

    private void OnActionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}