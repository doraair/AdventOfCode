namespace AdventOfCode.Day12
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    internal class PartTwo
    {
        private const string inputPath = "./Day12/input.txt";
        private char[][] map;
        private char[][] mapClear;

        internal void Run()
        {
            map = GetInput(inputPath);
            mapClear = map.Select(o => o.ToArray()).ToArray();

            var totalPrice = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '#')
                    {
                        continue;
                    }

                    var gardenPlots = new List<Point>();
                    var gardenPlotsEdge = new List<Point>();
                    var label = map[y][x];
                    if (mapClear[y][x] == label)
                    {

                        var perimeter = FindGardenPlots(new Point(x, y), map[y][x], ref gardenPlots, ref gardenPlotsEdge);
                        var regionOfArea = gardenPlots.Count;
                        // Console.WriteLine($"The type {label}. Region of Area: {regionOfArea}; Perimeter: {perimeter} ");
                        Console.WriteLine($"Total Price of {label}  {regionOfArea}*{perimeter}={regionOfArea * perimeter}");
                        totalPrice += regionOfArea * perimeter;
                        if (label == 'I')
                        {
                            FindingSide(gardenPlotsEdge);
                        }
                    }
                }
            }

            Console.WriteLine($"Total Price: {totalPrice}");
        }

        private int FindGardenPlots(
            Point gardenPlot,
            char label,
            ref List<Point> gardenPlots,
            ref List<Point> gardenPlotsEdge)
        {
            if (gardenPlots.Contains(gardenPlot))
            {
                return 0;
            }

            if (OutOfTheMap(gardenPlot))
            {
                return 1;
            }

            var currentHeight = map[gardenPlot.Y][gardenPlot.X];
            if (currentHeight != label)
            {
                return 1;
            }

            if (currentHeight == label)
            {
                gardenPlots.Add(gardenPlot);
                // mark as read
                mapClear[gardenPlot.Y][gardenPlot.X] = '#';
            }

            var up = FindGardenPlots(Move(gardenPlot, Movement.Up), label, ref gardenPlots, ref gardenPlotsEdge);
            var down = FindGardenPlots(Move(gardenPlot, Movement.Down), label, ref gardenPlots, ref gardenPlotsEdge);
            var left = FindGardenPlots(Move(gardenPlot, Movement.Left), label, ref gardenPlots, ref gardenPlotsEdge);
            var right = FindGardenPlots(Move(gardenPlot, Movement.Right), label, ref gardenPlots, ref gardenPlotsEdge);

            if (up == 1 || down == 1 || left == 1 || right == 1)
            {
                gardenPlotsEdge.Add(gardenPlot);
            }

            return up + down + left + right;
        }

        private void FindingSide(List<Point> gardenPlots)
        {
            gardenPlots.OrderBy(o => o.Y).ThenBy(o => o.X);

            foreach (var item in gardenPlots)
            {
                Console.WriteLine(item);
            }
        }

        private Point Move(Point currentPosition, Movement movement)
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

        private void PrintMap(char[][] map)
        {
            foreach (var line in map)
            {
                Console.WriteLine(new string(line));
            }
        }

        private char[][] GetInput(string path)
        {
            return File.ReadLines(path)
                .Select(line => line.ToArray()).ToArray();
        }
    }
}
