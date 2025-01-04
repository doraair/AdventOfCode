namespace AdventOfCode.Day02
{
    using System;
    using System.Linq;

    internal class PartOne
    {
        private string inputPath = "./Day02/input.txt";

        internal void Run()
        {
            var input = GetInput(inputPath);

            var reportNumber = 0;
            var reportDiffList = new Dictionary<int, int[]>();

            // Find Diff
            foreach (var levels in input)
            {
                var result = new List<int>();
                for (int i = 1; i < levels.Length; i++)
                {
                    var diff = levels[i] - levels[i - 1];
                    result.Add(diff);
                }
                reportDiffList.Add(reportNumber, result.ToArray());
                reportNumber++;
            }

            // Find Safe Reports
            var increingReport = reportDiffList.Where(r => r.Value.All(o => o < 0 && o > -4)).ToList();
            var decreingReport = reportDiffList.Where(r => r.Value.All(o => o > 0 && o < 4)).ToList();

            Console.WriteLine(increingReport.Count + decreingReport.Count);
        }

        private bool IsReportSafe(int[] levels)
        {
            bool isReportSafe = true;
            int index = -1;
            var reportDirection = GetReportDirection2(levels[0], levels[1]); ;
            for (int i = 0; i < levels.Length - 1; i++)
            {
                if (reportDirection == Direction.None)
                {
                    isReportSafe = false;
                    index = i;
                    break;
                }

                var diff = FindDiff(levels[i], levels[i + 1], reportDirection);
                if (diff <= 0)
                {
                    isReportSafe = false;
                    index = i;
                    break;
                }

                if (diff > 3)
                {
                    isReportSafe = false;
                    index = i;
                    break;
                }
            }
            return isReportSafe;
        }

        private static Direction GetReportDirection2(int currentLevel, int nextLevel)
        {
            Direction reportDirection;
            var diffForDirection = currentLevel - nextLevel;
            if (diffForDirection == 0)
            {
                reportDirection = Direction.None;
            }
            else if (diffForDirection > 0)
            {
                reportDirection = Direction.Decreasing;
            }
            else
            {
                reportDirection = Direction.Increasing;
            }

            return reportDirection;
        }

        private int FindDiff(int currentLevel, int nextLevel, Direction reportDirection)
        {
            var diff = 0;
            if (reportDirection == Direction.Decreasing)
            {
                diff = currentLevel - nextLevel;
            }
            else
            {
                diff = nextLevel - currentLevel;
            }
            return diff;
        }

        private int[][] GetInput(string path)
        {
            return File.ReadLines(path)
                .Select(line => line.Split(' '))
                .Select(values => values.Select(int.Parse).ToArray())
                .ToArray();
        }
    }
}
