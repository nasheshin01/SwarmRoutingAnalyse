using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Simulation;

namespace SimulationUI
{
    public partial class MapWindow : Window
    {
        private Random _randomizer;
        private int? _width;
        private int? _height;
        private int? _droneCount;
        private int[,] _map;

        private int? _startX;
        private int? _startY;
        private int? _endX;
        private int? _endY;

        public MapWindow(MapConfig mapConfig)
        {
            _randomizer = new Random(42);

            _width = mapConfig.MapSize.Width;
            _height = mapConfig.MapSize.Height;
            _droneCount = mapConfig.DroneCount;
            _map = mapConfig.Map;

            _startX = mapConfig.StartDronePoint.X;
            _startY = mapConfig.StartDronePoint.Y;
            _endX = mapConfig.EndDronePoint.X;
            _endY = mapConfig.EndDronePoint.Y;

            InitializeComponent();

            WidthBox.Text = _width.ToString();
            DroneCountBox.Text = _droneCount.ToString();
            StartXBox.Text = _startX.ToString();
            StartYBox.Text = _startY.ToString();
            EndXBox.Text = _endX.ToString();
            EndYBox.Text = _endY.ToString();
            
            DrawMap();
        }

        private Tuple<int, int> TransformXy(int x, int y)
        {
            return new Tuple<int, int>((int)((x / (float)_width) * 400 + 20), (int)((y / (float)(_height)) * 400 + 20));
        }

        private void DrawMap()
        {
            Canvas.Children.Clear();
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (_map[x, y] == 0)
                        continue;

                    var color = _map[x, y] switch
                    {
                        1 => Brushes.Black,
                        2 => Brushes.Green,
                        3 => Brushes.Red,
                        _ => Brushes.White
                    };
                    
                    var size = _map[x, y] switch
                    {
                        1 => 5,
                        2 => 15,
                        3 => 15,
                        _ => 0
                    };

                    var (absX, absY) = TransformXy(x, y);
                    var dot = new Ellipse
                        { Width = size, Height = size, Fill = color, Stroke = Brushes.Black, StrokeThickness = 1 };

                    Canvas.SetLeft(dot, absX - size / 2f);
                    Canvas.SetTop(dot, absY - size / 2f);
                    Canvas.Children.Add(dot);
                }
            }
        }

        private void GenerateMap()
        {
            _map = new int[400, 400];
            _map[(int)_startX, (int)_startY] = 2;
            _map[(int)_endX, (int)_endY] = 3;

            var spawnedDrones = 0;
            while (spawnedDrones < _droneCount - 2)
            {
                var x = _randomizer.Next(0, (int)_width);
                var y = _randomizer.Next(0, (int)_height);

                if (_map[x, y] != 0)
                    continue;

                _map[x, y] = 1;
                spawnedDrones++;
            }
        }

        private void OnWidthChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(WidthBox.Text, out var width))
            {
                _width = width;
                _height = width;
                return;
            }

            _width = null;
            _height = null;
        }

        private void OnDroneCountChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(DroneCountBox.Text, out var droneCount))
            {
                _droneCount = droneCount;
                return;
            }

            _droneCount = null;
        }

        private void OnDrawRandomMapButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_width == null || _height == null || _droneCount == null)
            {
                MessageBox.Show("Width, height or drone count is wrong", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            if (_startX == null || _startY == null || _endX == null || _endY == null)
                return;

            GenerateMap();
            DrawMap();
        }

        private void OnDrawStartEndButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_startX == null || _startY == null || _endX == null || _endY == null)
                return;
            
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    if (_map[x, y] == 2 || _map[x, y] == 3)
                        _map[x, y] = 0;

            
            _map[(int)_startX, (int)_startY] = 2;
            _map[(int)_endX, (int)_endY] = 3;
            DrawMap();
        }

        private void OnStartXChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(StartXBox.Text, out var startX))
            {
                _startX = startX;
                return;
            }

            _startX = null;
        }

        private void OnStartYChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(StartYBox.Text, out var startY))
            {
                _startY = startY;
                return;
            }

            _startY = null;
        }

        private void OnEndXChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(EndXBox.Text, out var endX))
            {
                _endX = endX;
                return;
            }

            _endX = null;
        }

        private void OnEndYChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(EndYBox.Text, out var endY))
            {
                _endY = endY;
                return;
            }

            _endY = null;
        }

        public MapConfig GetMapConfig()
        {
            return new MapConfig()
            {
                DroneCount = (int)_droneCount,
                MapSize = new System.Drawing.Size((int)_width, (int)_height),
                Map = _map,
                StartDronePoint = new System.Drawing.Point((int)_startX, (int)_startY),
                EndDronePoint = new System.Drawing.Point((int)_endX, (int)_endY)
            };
        }
    }
}