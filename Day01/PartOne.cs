namespace AdventOfCode.Day01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PartOne
    {
        private string inputPath = "./Day01/input.txt";
        private List<int> group1 = [];
        private List<int> group2 = [];

        internal void Run()
        {
            int[][] input = GetInput(inputPath);
            AssignToGroups(input);

            group1.Sort();
            group2.Sort();

            var distanceList = new List<int>();

            for (int i = 0; i < group1.Count; i++)
            {
                int distance = 0;
                var location1 = Math.Abs(group1[i]);
                var location2 = Math.Abs(group2[i]);
                distance = location1 - location2;

                distanceList.Add(Math.Abs(distance));
            }

            Console.WriteLine(distanceList.Sum());

        }

        private void AssignToGroups(int[][] input)
        {
            foreach (var item in input)
            {
                group1.Add(item[0]);
                group2.Add(item[1]);
            }
        }
        private int[][] GetInput(string path)
        {
            return File.ReadLines(path)
                .Select(line => line.Split("   "))
                .Select(values => values.Select(int.Parse).ToArray())
                .ToArray();
        }
    }
}
