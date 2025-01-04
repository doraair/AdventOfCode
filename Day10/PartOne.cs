namespace AdventOfCode.Day10
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    internal class PartOne
    {
        private const string inputPath = "./Day10/input.txt";
        private readonly string[] map;
        internal PartOne()
        {
            map = GetInput(inputPath);
        }

        public void Run()
        {
            var hikingTrails = new List<(Point location, int score, int rating)>();
            for (int y = 0; y < map.Length; y++)
            {
                var line = map[y];
                var trailheadIndex = line.IndexOf('0');
                while (trailheadIndex != -1)
                {
                    var trailhead = new Point(trailheadIndex, y);
                    var topPeaks = new List<Point>();
                    var totalRoute = FindHikingTrails(trailhead, 0, ref topPeaks);
                    var score = topPeaks.Distinct().Count();
                    hikingTrails.Add((trailhead, score, totalRoute));

                    trailheadIndex++;
                    if (trailheadIndex == line.Length)
                    {
                        break;
                    }
                    trailheadIndex = line.IndexOf('0', trailheadIndex);
                }
            }

            foreach (var item in hikingTrails)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Part Two Answer - Total Rating hiking trails - {hikingTrails.Sum(o => o.rating)}");
            Console.WriteLine($"Part One Answer - Total Score hiking trails - {hikingTrails.Sum(o => o.score)}");
        }

        public int FindHikingTrails(
            Point trailhead,
            int height,
            ref List<Point> topPeaks)
        {
            if (OutOfTheMap(trailhead))
            {
                return 0;
            }

            int currentHeight = -1;
            if (!int.TryParse(map[trailhead.Y][trailhead.X].ToString(), out currentHeight))
            {
                return 0;
            }
            if (currentHeight != height)
            {
                return 0;
            }

            if (currentHeight == 9)
            {
                topPeaks.Add(trailhead);
                return 1;
            }

            return FindHikingTrails(Move(trailhead, Movement.Up), height + 1, ref topPeaks) +
                FindHikingTrails(Move(trailhead, Movement.Down), height + 1, ref topPeaks) +
                FindHikingTrails(Move(trailhead, Movement.Left), height + 1, ref topPeaks) +
                FindHikingTrails(Move(trailhead, Movement.Right), height + 1, ref topPeaks);
        }

        public Point Move(Point currentPosition, Movement movement)
        {
            var x = 0; var y = 0;
            switch (movement)
            {
                case Movement.Up:
                    y--;
                    break;
                case Movement.Down:
                    y++;
                    break;
                case Movement.Left:
                    x--;
                    break;
                case Movement.Right:
                    x++;
                    break;
            }

            return new Point(currentPosition.X + x, currentPosition.Y + y);
        }

        private bool OutOfTheMap(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= map[0].Length || position.Y >= map.Length;
        }

        private string[] GetInput(string path)
        {
            return File.ReadLines(path).ToArray();
        }
    }
}
