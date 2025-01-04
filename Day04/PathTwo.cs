namespace AdventOfCode.Day04
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class PartTwo
    {
        private const string inputPath = "./Day04/input.txt";

        private int[][] directions = new int[][]
        {
            //  row, column
            // new int[] { 0, 1 }, // right
            // new int[] { 0, -1 }, // left
            new int[] { 1, 0 }, // down
            new int[] { -1, 0 }, // up
            new int[] { 1, 1 }, // diagonal down-right
            new int[] { 1, -1 }, // diagonal down-left
            new int[] { -1, 1 }, // diagonal up-right
            new int[] { -1, -1 } // diagonal up-left
        };

        private int[][] crossDirection = new int[][]
        {
            //  row, column
            [-1, -1], // diagonal top left
            [1, 1], //  diagonal down-right
            [-1, 1], // diagonal top right
            [1, -1], // diagonal down-left
        };

        internal void Run()
        {
            var input = GetInput(inputPath);
            var total = 0;

            var totalLine = input.Count();
            var colLength = input[0].Length;
            for (var rowIndex = 0; rowIndex < totalLine; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < colLength; columnIndex++)
                {
                    if (input[rowIndex][columnIndex] == 'A')
                    {
                        total += CountXMas(input, rowIndex, columnIndex);
                    }
                }
            }

            Console.WriteLine(total);
        }

        private int CountXMas(
         string[] input,
         int rowIndex,
         int columnIndex)
        {
            var rows = input.Length;
            var cols = input[rowIndex].Length;

            var sCount = 0;
            var mCount = 0;

            char topLeft = '-';
            char downRight = '+';

            var i = 0;
            foreach (var direction in crossDirection)
            {
                var rowDirection = direction[0];
                var columnDirection = direction[1];

                var currentRow = rowIndex + rowDirection;
                var currentColumn = columnIndex + columnDirection;

                if (currentRow < 0 || currentColumn < 0
                    || currentRow >= rows || currentColumn >= cols)
                {
                    break;
                }

                var currentChar = input[currentRow][currentColumn];
                if (currentChar != 'S' && currentChar != 'M')
                {
                    break;
                }

                if (currentChar == 'S')
                {
                    sCount++;
                }
                if (currentChar == 'M')
                {
                    mCount++;
                }

                if (i == 0)
                {
                    topLeft = currentChar;
                }
                if (i == 1)
                {
                    downRight = currentChar;
                }
                if (i == 2 && topLeft == downRight)
                {
                    break;
                }
                i++;
            }

            if (topLeft == downRight)
            {
                return 0;
            }
            return mCount == 2 && sCount == 2 ? 1 : 0;
        }

        private string[] GetInput(string path)
        {
            return File.ReadLines(path).Select(o => o).ToArray();
        }
    }
}
