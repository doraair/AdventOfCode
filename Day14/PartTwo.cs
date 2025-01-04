namespace AdventOfCode.Day14
{
    using System.Drawing;
    using System.Linq;

    internal class PartTwo
    {
        private const string input = "./Day14/input.txt";
        private const int mapWide = 101;
        private const int mapHeight = 103;
        public void Run()
        {
            var robots = GetInput();
            for (int i = 6446; i < mapWide * mapHeight; i++)
            {
                var lastPositions = new Dictionary<int, Point>();
                var robotName = 0;

                foreach (var robot in robots)
                {
                    var lastPosition = GetLastPosition(robot, i);
                    lastPositions.Add(robotName, lastPosition);
                    robotName++;
                }
                var currentRobots = lastPositions.Select(o => o.Value).ToList();
                var largestArea = GetLargestArea(currentRobots);
                if (largestArea > 100)
                {
                    PrintMap(currentRobots);
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        private static Point GetLastPosition(Robot robot, int seconds)
        {
            var x = (robot.StartingPoint.X + (robot.Velocities.X * seconds)) % mapWide;
            var y = (robot.StartingPoint.Y + (robot.Velocities.Y * seconds)) % mapHeight;

            if (x < 0)
            {
                x = mapWide + x;
            }

            if (y < 0)
            {
                y = mapHeight + y;
            }

            if (x >= mapWide || y >= mapHeight)
            {
                Console.WriteLine($"Robot {robot.StartingPoint} with velocity {robot.Velocities} is out of map");
            }

            return new Point(x, y);
        }

        private void PrintMap(List<Point> robots)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWide; x++)
                {
                    if (robots.Any(o => o == new Point(x, y)))
                    {
                        //var count = robots.Count(o => o == new Point(x, y));
                        //Console.Write($"{count}");
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private int GetLargestArea(List<Point> positions)
        {
            var visited = new List<Point>();
            var largestGarden = 0;
            foreach (var position in positions)
            {
                var gardenPlots = new List<Point>();
                FindArea(position, ref positions, ref visited, ref gardenPlots);
                var gardenSize = gardenPlots.Count;
                if (gardenSize > largestGarden)
                {
                    largestGarden = gardenSize;
                }
            }
            return largestGarden;
        }

        private void FindArea(
            Point gardenPlot,
            ref List<Point> robots,
            ref List<Point> visited,
            ref List<Point> area)
        {
            if (visited.Contains(gardenPlot))
            {
                return;
            }

            if (OutOfTheMap(gardenPlot))
            {
                return;
            }
            var isFound = robots.Any(o => o == gardenPlot);
            if (!isFound)
            {
                return;
            }

            if (isFound)
            {
                area.Add(gardenPlot);
                visited.Add(gardenPlot);
            }

            FindArea(PointExtension.Move(gardenPlot, Movement.Up), ref robots, ref visited, ref area);
            FindArea(PointExtension.Move(gardenPlot, Movement.Down), ref robots, ref visited, ref area);
            FindArea(PointExtension.Move(gardenPlot, Movement.Left), ref robots, ref visited, ref area);
            FindArea(PointExtension.Move(gardenPlot, Movement.Right), ref robots, ref visited, ref area);
        }

        private bool OutOfTheMap(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= mapWide || position.Y >= mapHeight;
        }

        private List<Robot> GetInput()
        {
            var robotProps = File.ReadLines(input)
                .Select(line => line.Split(' ')).ToArray();

            var robots = new List<Robot>();
            for (int i = 0; i < robotProps.Length; i++)
            {
                var robotProp = robotProps[i];
                var position = robotProp[0].Substring(2).Split(',');
                var velocities = robotProp[1].Substring(2).Split(',');
                var robot = new Robot()
                {
                    StartingPoint = new Point(int.Parse(position[0]), int.Parse(position[1])),
                    Velocities = new Point(int.Parse(velocities[0]), int.Parse(velocities[1]))
                };
                robots.Add(robot);
            }

            return robots;
        }
    }
}
