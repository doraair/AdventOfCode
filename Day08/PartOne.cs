namespace AdventOfCode.Day08
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class PartOne
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
                    var locations = GetAntinodeLocations(antenna, frequency.Select(o => o.Position).ToList());
                    antinodeLocations.AddRange(locations);
                }
            }
            Console.WriteLine($"Antinode locations: {antinodeLocations.GroupBy(o => o).Count()}");
        }


        internal List<Point> GetAntinodeLocations(
        Antenna antinode,
        List<Point> pairingLocations)
        {
            var antinodeLocations = new List<Point>();
            foreach (var pairAntenna in pairingLocations)
            {
                if (antinode.Position == pairAntenna)
                {
                    // Skip the same antenna
                    continue;
                }

                var location = GetAntinodeLocation(antinode.Position, pairAntenna);
                if (OutOfTheMap(location))
                {
                    continue;
                }
                antinodeLocations.Add(location);
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
