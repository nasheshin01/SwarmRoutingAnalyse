using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Simulation;

namespace SimulationUI
{
    public partial class MainWindow : Window
    {
        private SimulationConfig _simulationConfig;
        private bool _simConfigReady; // TODO: жуткий костыль, удалить!!!!
        
        public MapConfig MapConfig { get; set; }

        private MainSimulation _mainSimulation;
        
        public MainWindow()
        {
            InitializeComponent();
            
            MapConfig = MapConfig.GetDefaultMapConfig();
            _simulationConfig = new SimulationConfig() { MapConfig = MapConfig };
            
            ScoutCountBox.Text = _simulationConfig.ScoutCount.ToString();
            DataCountBox.Text = _simulationConfig.DataCount.ToString();
            DataGeneratePeriodBox.Text = _simulationConfig.DataGeneratePeriod.ToString();
            DataSizeBox.Text = _simulationConfig.DataSize.ToString();
            PackageSizeBox.Text = _simulationConfig.PackageSize.ToString();
            ScoutSizeBox.Text = _simulationConfig.ScoutSize.ToString();
            ScoutEnergyLimitBox.Text = _simulationConfig.ScoutEnergyLimit.ToString();
            MaxDroneDistanceBox.Text = _simulationConfig.MaxDroneDistance.ToString();
            DroneMovePeriodBox.Text = _simulationConfig.DroneMovePeriod.ToString();
            EndSimulationTickBox.Text = _simulationConfig.EndSimulationTick.ToString();
            ConstLoadingSpeedBox.Text = _simulationConfig.ConstLoadingSpeed.ToString();

            _simConfigReady = true;
        }

        // Event handler for the "Add Rule" button
        private void BtnAddRule_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the AddRuleWindow
            var addRuleWindow = new RuleEditorWindow(_simulationConfig.Rules);

            // Show the AddRuleWindow as a dialog and wait for it to close
            var result = addRuleWindow.ShowDialog();

            _simulationConfig.Rules = addRuleWindow.GetResultRules();
        }

        private void OnBtnEditMapClicked(object sender, RoutedEventArgs e)
        {
            var mapWindow = new MapWindow(MapConfig);

            // Show the AddRuleWindow as a dialog and wait for it to close
            var result = mapWindow.ShowDialog();

            MapConfig = mapWindow.GetMapConfig();

            _simulationConfig.MapConfig = MapConfig;
        }

        private void OnRunSimulationButtonClicked(object sender, RoutedEventArgs e)
        {
            RichTxtBoxLogs.Document.Blocks.Clear();
            
            _mainSimulation = new MainSimulation(_simulationConfig);
            _mainSimulation.LogEventHandler += OnSimulationLogArrived;

            var simulationTask = Task.Run(_mainSimulation.Run);
        }

        private void OnSimulationLogArrived(object? sender, SimulationLogEventArgs simulationLogEventArgs)
        {
            if (simulationLogEventArgs.LogType is LogType.ScoutMove or LogType.WorkerMove)
                return;
            
            RichTxtBoxLogs.Dispatcher.Invoke(() =>
            {
                RichTxtBoxLogs.Document.Blocks.Add(
                    new Paragraph(new Run($"{simulationLogEventArgs.Tick}, {simulationLogEventArgs.LogMessage}")));
            });
        }

        private void OnSettingsChanged(object sender, TextChangedEventArgs e)
        {
            if (!_simConfigReady)
                return;
            
            int.TryParse(ScoutCountBox.Text, out _simulationConfig.ScoutCount);
            int.TryParse(DataCountBox.Text, out _simulationConfig.DataCount);
            float.TryParse(DataGeneratePeriodBox.Text, out _simulationConfig.DataGeneratePeriod);
            int.TryParse(DataSizeBox.Text, out _simulationConfig.DataSize);
            int.TryParse(PackageSizeBox.Text, out _simulationConfig.PackageSize);
            int.TryParse(ScoutSizeBox.Text, out _simulationConfig.ScoutSize);
            float.TryParse(ScoutEnergyLimitBox.Text, out _simulationConfig.ScoutEnergyLimit);
            float.TryParse(MaxDroneDistanceBox.Text, out _simulationConfig.MaxDroneDistance);
            float.TryParse(DroneMovePeriodBox.Text, out _simulationConfig.DroneMovePeriod);
            float.TryParse(EndSimulationTickBox.Text, out _simulationConfig.EndSimulationTick);
            float.TryParse(ConstLoadingSpeedBox.Text, out _simulationConfig.ConstLoadingSpeed);
        }
    }
}