namespace AdventOfCode.Day08
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class PartTwo
    {
        private const string inputPath = "./Day08/input.txt";
        private string[] map;

        internal void Run()
        {
            map = GetInput(inputPath);
            var antinodeLocations = new List<Point>();
            var antennaLocations = FindingAntenna(map);

            var antennaGroupByFrequencyLocations = antennaLocations
                .GroupBy(o => o.Frequency)
                .ToList();
            foreach (var frequency in antennaGroupByFrequencyLocations)
            {
                Console.WriteLine($"{frequency.Key}-frequency antennas");

                foreach (var antenna in frequency)
                {
                    var locations = GetAntinodeLocations(antenna, frequency.ToList());
                    antinodeLocations.AddRange(locations);
                }
            }

            Console.WriteLine($"Antinode locations: {antinodeLocations.Count()}");
            var uniqueAntinodeLocations = antinodeLocations.OrderBy(o => o.Y).Distinct();

            foreach (var frequency in uniqueAntinodeLocations)
            {
                Console.WriteLine($"{frequency}");
            }
            var total = uniqueAntinodeLocations.Count();// + uniqueAntinodeLocationsForAntinode.Count();
            Console.WriteLine($"Unique Antinode locations: {total}");
        }

        internal List<Point> GetAntinodeLocations(
        Antenna antenna,
        List<Antenna> pairingLocations)
        {
            var antinodeLocations = new List<Point>();
            foreach (var pairAntenna in pairingLocations)
            {
                if (antenna == pairAntenna)
                {
                    // Skip the same antenna
                    continue;
                }

                var mainAntenna = antenna.Position;
                var pairAntennaLocation = pairAntenna.Position;
                antinodeLocations.Add(mainAntenna);
                Point antinodeLocation;
                while (true)
                {
                    antinodeLocation = GetAntinodeLocation(mainAntenna, pairAntennaLocation);
                    if (OutOfTheMap(antinodeLocation))
                    {
                        break;
                    }

                    antinodeLocations.Add(antinodeLocation);
                    pairAntennaLocation = mainAntenna;
                    mainAntenna = antinodeLocation;
                }
            }

            return antinodeLocations;
        }

        internal Point GetAntinodeLocation(
            Point antenna,
            Point pairAntenna)
        {
            var xLocation = antenna.X - pairAntenna.X;
            var yLocation = antenna.Y - pairAntenna.Y;

            return new Point(antenna.X + xLocation, antenna.Y + yLocation);
        }

        internal List<Antenna> FindingAntenna(string[] input)
        {
            var antennaLocations = new List<Antenna>();
            string pattern = @"[a-zA-Z0-9]";

            for (var y = 0; y < input.Length; y++)
            {
                var locations = Regex.Matches(input[y], pattern);
                foreach (Match location in locations)
                {
                    var antenna = new Antenna() { Frequency = location.Value, Position = new Point(location.Index, y) };
                    antennaLocations.Add(antenna);
                }
            }

            return antennaLocations;
        }

        private bool OutOfTheMap(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= map[0].Length || position.Y >= map.Length;
        }

        private string[] GetInput(string path)
        {
            return File.ReadLines(path).Select(o => o).ToArray();
        }
    }
}
