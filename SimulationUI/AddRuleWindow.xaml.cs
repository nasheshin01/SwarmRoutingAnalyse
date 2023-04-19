using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AddRuleWindow : Window
    {
        public string AgentType { get; set; }
        // Properties for the variable, equation, constant, and action
        public string Variable { get; set; }
        public string Equation { get; set; }
        public double Constant { get; set; }
        public string Action { get; set; }

        public AddRuleWindow()
        {
            InitializeComponent();
        }

        // Event handler for the "Add Rule" button click
        private void BtnAddRule_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrEmpty(cmbVariable.Text) || string.IsNullOrEmpty(cmbEquation.Text) ||
                string.IsNullOrEmpty(txtConstant.Text) || string.IsNullOrEmpty(cmbAction.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update properties with selected values
            AgentType = cmbAgentType.Text;
            Variable = cmbVariable.Text;
            Equation = cmbEquation.Text;
            Constant = Convert.ToDouble(txtConstant.Text);
            Action = cmbAction.Text;

            // Close the window with "OK" result
            DialogResult = true;
        }

        // Event handler for the "Cancel" button click
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the window with "Cancel" result
            DialogResult = false;
        }
    }
}