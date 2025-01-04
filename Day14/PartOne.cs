namespace AdventOfCode.Day14
{
    using System.Drawing;
    using System.Linq;

    internal class PartOne
    {
        private const string input = "./Day14/input.txt";
        private const int mapWide = 101;
        private const int mapHeight = 103;
        public void Run()
        {
            var robots = GetInput();

            //PrintMap(robots.Select(o => o.StartingPoint).ToList());
            Console.WriteLine("====================================");
            var lastPositions = new Dictionary<int, Point>();
            var robotName = 0;
            foreach (var robot in robots)
            {
                var lastPosition = GetLastPosition(robot, 100);
                lastPositions.Add(robotName, lastPosition);
                robotName++;
            }
            //PrintMap(lastPositions.Select(o => o.Value).ToList());
            Console.WriteLine($"Total {GetTotalRobots(lastPositions)}");
        }

        private Point GetLastPosition(Robot robot, int seconds)
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

        private int GetTotalRobots(Dictionary<int, Point> robots)
        {
            var quadrant1 = 0;
            var quadrant2 = 0;
            var quadrant3 = 0;
            var quadrant4 = 0;

            var halfMapWide = (mapWide - 1) / 2;
            var halfMapHeight = (mapHeight - 1) / 2;

            quadrant1 = robots.Where(r =>
            r.Value.X >= 0 && r.Value.X < halfMapWide
            && r.Value.Y >= 0 && r.Value.Y < halfMapHeight
            ).Count();

            quadrant2 = robots.Where(r =>
            r.Value.X > halfMapWide
            && r.Value.Y >= 0 && r.Value.Y < halfMapHeight
            ).Count();

            quadrant3 = robots.Where(r =>
            r.Value.X >= 0 && r.Value.X < halfMapWide
            && r.Value.Y > halfMapHeight
            ).Count();

            quadrant4 = robots.Where(r =>
            r.Value.X > halfMapWide
            && r.Value.Y > halfMapHeight
            ).Count();

            return quadrant1 * quadrant2 * quadrant3 * quadrant4;
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

        private void PrintMap(List<Point> robots)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWide; x++)
                {
                    if (robots.Any(o => o == new Point(x, y)))
                    {
                        var count = robots.Count(o => o == new Point(x, y));
                        Console.Write($"{count}");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}