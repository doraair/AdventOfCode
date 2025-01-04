namespace AdventOfCode.Day04
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class PartOne
    {
        private const string inputPath = "./Day04/input.txt";

        private string xmasPattern = @"XMAS";
        private string samxPattern = @"SAMX";

        private int[][] directions = new int[][]
        {
            //  row, column
            // new int[] { 0, 1 }, // right
            // new int[] { 0, -1 }, // left
            [1, 0], // down
            [-1, 0], // up
            [1, 1], // diagonal down-right
            [1, -1], // diagonal down-left
            [-1, 1], // diagonal up-right
            [-1, -1] // diagonal up-left
        };

        internal void Run()
        {
            var input = GetInput(inputPath);
            var total = 0;
            var options = RegexOptions.Multiline;
            foreach (var line in input)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var xmasMatches = Regex.Matches(line, xmasPattern, options);
                    var samxMatches = Regex.Matches(line, samxPattern, options);
                    total += xmasMatches.Count;
                    total += samxMatches.Count;
                }
            }

            var totalLine = input.Count();
            var colLength = input[0].Length;
            for (var rowIndex = 0; rowIndex < totalLine; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < colLength; columnIndex++)
                {
                    total += CountWords(input, rowIndex, columnIndex, "XMAS");
                }
            }

            Console.WriteLine(total);
        }

        private int CountWords(
            string[] input,
            int rowIndex,
            int columnIndex,
            string findingWord)
        {
            var wordLength = findingWord.Length;
            var rows = input.Length;
            var cols = input[rowIndex].Length;

            var matchCount = 0;
            foreach (var direction in directions)
            {
                var currentRow = rowIndex;
                var currentColumn = columnIndex;

                var rowDirection = direction[0];
                var columnDirection = direction[1];

                for (var w = 0; w < wordLength; w++)
                {
                    if (currentRow < 0 || currentColumn < 0
                        || currentRow >= rows || currentColumn >= cols)
                    {
                        break;
                    }

                    if (input[currentRow][currentColumn] != findingWord[w])
                    {
                        break;
                    }

                    if (w == wordLength - 1)
                    {
                        matchCount++;
                    }
                    currentRow += rowDirection;
                    currentColumn += columnDirection;
                }
            }
            return matchCount;
        }

        private string[] GetInput(string path)
        {
            return File.ReadLines(path).Select(o => o).ToArray();
        }
    }
}
