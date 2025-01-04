namespace AdventOfCode.Day03
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class PartTwo
    {
        private string inputPath = "./Day03/input.txt";
        private string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        private string doAndDont = @"do\(\).*?(don't\(\)|$)";

        internal void Run()
        {
            var input = GetInput(inputPath);
            var total = 0;
            var lines = string.Join("", input.ToArray());

            var fistIndexOfDo = lines.IndexOf("do()");
            var fistIndexOfDont = lines.IndexOf("don't()");
            var splitIdex = Math.Min(fistIndexOfDo, fistIndexOfDont);
            var lineArry = Math.Min(fistIndexOfDo, fistIndexOfDont);

            var firstSentence = lines.Substring(0, splitIdex);
            var lastSentence = lines.Substring(splitIdex);


            var matches = Regex.Matches(firstSentence, pattern);
            total += GetMultiplyTotal(firstSentence);

            matches = Regex.Matches(lastSentence, doAndDont);
            foreach (Match match in matches)
            {
                total += GetMultiplyTotal(match.Value);
            }

            Console.WriteLine(total);
        }

        private int GetMultiplyTotal(string line)
        {
            var total = 0;
            var matches = Regex.Matches(line, pattern);
            foreach (Match match in matches)
            {
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);
                int result = x * y;
                total += result;
                Console.WriteLine($"mul({x},{y}) = {result}");
            }
            return total;
        }

        private List<string> GetInput(string path)
        {
            return File.ReadLines(path).Select(o => o).ToList();
        }
    }
}
