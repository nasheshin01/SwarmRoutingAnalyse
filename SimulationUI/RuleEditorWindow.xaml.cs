using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Simulation;

namespace SimulationUI;

public partial class RuleEditorWindow
{
    private readonly ObservableCollection<RuleControl> _ruleControls;

    public RuleEditorWindow(List<Rule> rules)
    {
        InitializeComponent();

        if (rules.Count == 0)
        {
            _ruleControls = new ObservableCollection<RuleControl> { new("KekRule") };
        }
        else
        {
            _ruleControls = new ObservableCollection<RuleControl>();
            foreach (var rule in rules)
            {
                var ruleControl = new RuleControl(rule);
                _ruleControls.Add(ruleControl);
            }
        }
        RulesList.ItemsSource = _ruleControls;
    }

    public List<Rule> GetResultRules()
    {
        return _ruleControls.Select(rc => rc.GetResultRule()).ToList();
    }
    
    private void OnAddRuleButtonClicked(object sender, RoutedEventArgs e)
    {
        _ruleControls.Add(new RuleControl("KekRule1"));
    }
}