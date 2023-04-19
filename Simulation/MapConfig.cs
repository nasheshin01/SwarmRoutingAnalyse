using System.Drawing;

namespace Simulation;

public class MapConfig
{
    public Size MapSize { get; set; }
    public int DroneCount { get; set; }
    public Point StartDronePoint { get; set; }
    public Point EndDronePoint { get; set; }
    public int[,] Map { get; set; }

    public static MapConfig GetDefaultMapConfig()
    {
        var mapConfig = new MapConfig
        {
            MapSize = new Size(10, 10),
            DroneCount = 2,
            StartDronePoint = new Point(0, 0),
            EndDronePoint = new Point(9, 9),
            Map = new int[400, 400]
        };

        mapConfig.Map[mapConfig.StartDronePoint.X, mapConfig.StartDronePoint.Y] = 2;
        mapConfig.Map[mapConfig.EndDronePoint.X, mapConfig.EndDronePoint.Y] = 3;
        return mapConfig;
    }
}