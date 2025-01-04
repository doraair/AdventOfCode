namespace AdventOfCode.Day01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PartTwo
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
                var location = group1[i];
                var totalInGroup2 = group2.Count(o => o == location);

                distanceList.Add(location * totalInGroup2);
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
