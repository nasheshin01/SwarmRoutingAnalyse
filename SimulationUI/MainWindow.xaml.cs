using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using Simulation;
using Newtonsoft.Json;

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

            _simulationConfig = JsonConvert.DeserializeObject<SimulationConfig>(
                File.ReadAllText(@"settings.json"));
            MapConfig = _simulationConfig.MapConfig;

            ScoutCountBox.Text = _simulationConfig.ScoutCount.ToString();
            DataGeneratePeriodBox.Text = _simulationConfig.DataGeneratePeriod.ToString();
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
            var jsonSimulationConfig = JsonConvert.SerializeObject(_simulationConfig, Formatting.Indented);
            using var writer = new StreamWriter(@"settings.json");
            writer.Write(jsonSimulationConfig);
            writer.Close();

            const string pythonPath = @"D:\HSE\Diplom\SwarmAIProject(.NET)\SwarmAI\MesaSimulation\vizsim_main.py";
            
            var start = new ProcessStartInfo
            {
                FileName = "python", // path to python executable
                Arguments = pythonPath, // path to your python script
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(start);
            using var reader = process?.StandardOutput;
            var result = reader?.ReadToEnd();
        }

        private void OnSettingsChanged(object sender, TextChangedEventArgs e)
        {
            if (!_simConfigReady)
                return;
            
            int.TryParse(ScoutCountBox.Text, out _simulationConfig.ScoutCount);
            float.TryParse(DataGeneratePeriodBox.Text, out _simulationConfig.DataGeneratePeriod);
            int.TryParse(PackageSizeBox.Text, out _simulationConfig.PackageSize);
            int.TryParse(ScoutSizeBox.Text, out _simulationConfig.ScoutSize);
            float.TryParse(ScoutEnergyLimitBox.Text, out _simulationConfig.ScoutEnergyLimit);
            float.TryParse(MaxDroneDistanceBox.Text, out _simulationConfig.MaxDroneDistance);
            float.TryParse(DroneMovePeriodBox.Text, out _simulationConfig.DroneMovePeriod);
            float.TryParse(EndSimulationTickBox.Text, out _simulationConfig.EndSimulationTick);
            float.TryParse(ConstLoadingSpeedBox.Text, out _simulationConfig.ConstLoadingSpeed);
        }

        private void OnSaveMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != true) 
                return;
            
            var jsonSimulationConfig = JsonConvert.SerializeObject(_simulationConfig, Formatting.Indented);
            using var writer = new StreamWriter(saveFileDialog.FileName);
            writer.Write(jsonSimulationConfig);
            writer.Close();
        }

        private void OnLoadMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) 
                return;
            
            _simulationConfig = new SimulationConfig() { MapConfig = MapConfig };

            _simulationConfig = JsonConvert.DeserializeObject<SimulationConfig>(
                File.ReadAllText(openFileDialog.FileName));
            MapConfig = _simulationConfig.MapConfig;
            
            ScoutCountBox.Text = _simulationConfig.ScoutCount.ToString();
            DataGeneratePeriodBox.Text = _simulationConfig.DataGeneratePeriod.ToString();
            PackageSizeBox.Text = _simulationConfig.PackageSize.ToString();
            ScoutSizeBox.Text = _simulationConfig.ScoutSize.ToString();
            ScoutEnergyLimitBox.Text = _simulationConfig.ScoutEnergyLimit.ToString();
            MaxDroneDistanceBox.Text = _simulationConfig.MaxDroneDistance.ToString();
            DroneMovePeriodBox.Text = _simulationConfig.DroneMovePeriod.ToString();
            EndSimulationTickBox.Text = _simulationConfig.EndSimulationTick.ToString();
            ConstLoadingSpeedBox.Text = _simulationConfig.ConstLoadingSpeed.ToString();
        }

        private void OnPureRunSimulationButtonClicked(object sender, RoutedEventArgs e)
        {
            var jsonSimulationConfig = JsonConvert.SerializeObject(_simulationConfig, Formatting.Indented);
            using var writer = new StreamWriter(@"settings.json");
            writer.Write(jsonSimulationConfig);
            writer.Close();

            const string pythonPath = @"D:\HSE\Diplom\SwarmAIProject(.NET)\SwarmAI\MesaSimulation\sim_main.py";
            
            var start = new ProcessStartInfo
            {
                FileName = "python", // path to python executable
                Arguments = pythonPath, // path to your python script
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(start);
            while (process is { HasExited: false })
                Thread.Sleep(100);

            using (var reader = new StreamReader("out.txt"))
            {
                var receivedCount = int.Parse(reader.ReadLine());
                var lostCount = int.Parse(reader.ReadLine());
                var notSentCount = int.Parse(reader.ReadLine());
                var averagePackageTime = float.Parse(reader.ReadLine().Replace('.', ','));
                var lossPercentage = lostCount / (float)(receivedCount + lostCount + notSentCount);
                var notSentPercentage = notSentCount / (float)(receivedCount + lostCount + notSentCount);
                var receivedPercentage = receivedCount / (float)(receivedCount + lostCount + notSentCount);
                var algorithmCapacity = _simulationConfig.DataGeneratePeriod / averagePackageTime;
                var algorithmQuality = (receivedPercentage + (1 - 1 / algorithmCapacity)) / 2;
                
                RichTxtBoxResult.Document.Blocks.Clear();
                var resultString = "";
                resultString += "Кол-во доставленных пакетов: " + receivedCount + "\n";
                resultString += "Кол-во потерянных пакетов: " + lostCount + "\n";
                resultString += "Кол-во не отправленных пакетов: " + lostCount + "\n";
                resultString += "Среднее время отправки пакета: " + averagePackageTime + "\n";
                resultString += $"Было потеряно {Math.Round(lossPercentage * 100, 2)}% пакетов. \n";
                resultString += $"Не отправлено {Math.Round(notSentPercentage * 100, 2)}% пакетов. \n";
                if (lossPercentage + notSentPercentage > 0.2)
                    resultString += $"Проблема алгоритма: слишком мало пакетов доставлено. \n";
                
                if (averagePackageTime > _simulationConfig.DataGeneratePeriod)
                {
                    resultString += "Среднее время отправки пакета больше периода генерации данных.";
                    resultString += "У алгоритма низкая пропускная способность\n";
                }

                resultString += $"Качество алгоритма: {Math.Round(algorithmQuality * 100, 2)}%";
                RichTxtBoxResult.AppendText(resultString);
            }
        }
    }
}