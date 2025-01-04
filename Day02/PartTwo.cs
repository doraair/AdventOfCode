namespace AdventOfCode.Day02
{
    using System;
    using System.Linq;

    internal class PartTwo
    {
        private string inputPath = "./Day02/input.txt";

        public void Run()
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

            var safeReportIndexList = new List<int>();
            safeReportIndexList.AddRange(increingReport.Select(o => o.Key));
            safeReportIndexList.AddRange(decreingReport.Select(o => o.Key));

            // Find Unsafe Reports
            var unsefesReports = reportDiffList.Where(o => !safeReportIndexList.Contains(o.Key)).ToList();

            reportNumber = 0;
            var reSafeReportIndexList = new List<int>();
            foreach (var unsefesReport in unsefesReports)
            {
                reportNumber = unsefesReport.Key + 1;
                int[] levels = input[unsefesReport.Key];

                var isReportSafe = IsReportSafe(levels);
                if (!isReportSafe)
                {
                    var maxRetry = levels.Length - 1;
                    var tryRemoveIndexAt = 0;
                    while (!isReportSafe && tryRemoveIndexAt <= maxRetry)
                    {
                        var newlevels = levels.ToList();
                        newlevels.RemoveAt(tryRemoveIndexAt);

                        isReportSafe = IsReportSafe(newlevels.ToArray());

                        tryRemoveIndexAt++;
                    }
                }


                if (isReportSafe)
                {
                    var reportDiff = reportDiffList[unsefesReport.Key];
                    reSafeReportIndexList.Add(reportNumber);
                    Console.WriteLine($"Report Number: {unsefesReport.Key + 1}");
                    Console.WriteLine(string.Join(" ", levels));
                    Console.WriteLine(string.Join(" ", reportDiff));
                    Console.WriteLine($"Is Report Safe: {isReportSafe}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine(reSafeReportIndexList.Count);
            Console.WriteLine(safeReportIndexList.Count);
            Console.WriteLine($"Total Report Safe: {safeReportIndexList.Count + reSafeReportIndexList.Count}");
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
