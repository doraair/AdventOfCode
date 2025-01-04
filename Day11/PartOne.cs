namespace AdventOfCode.Day11
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PartOne
    {
        private const string inputPath = "./Day11/input.txt";

        internal void Run()
        {
            var stones = GetInput(inputPath);
            var blinks = 25;
            List<ulong> afterBlink = new List<ulong>();

            //Print(stones);
            for (var blink = 0; blink < blinks; blink++)
            {
                afterBlink.Clear();
                foreach (var stone in stones)
                {
                    var newStones = ApplyRules(stone);
                    afterBlink.AddRange(newStones);
                }
                // Print(afterBlink);
                stones = afterBlink.ToList();
            }

            Console.WriteLine(stones.Count());
        }

        internal void QuestionImprovement()
        {
            var input = GetInput(inputPath);
            var blinks = 75;
            Dictionary<ulong, long> afterBlink;

            Dictionary<ulong, long> stones = new Dictionary<ulong, long>();
            input.GroupBy(o => o).ToList().ForEach(o => stones.Add(o.Key, o.Count()));

            for (var blink = 0; blink < blinks; blink++)
            {
                afterBlink = new Dictionary<ulong, long>(); ;
                foreach (var stone in stones)
                {
                    var newStones = ApplyRules(stone.Key);
                    AddOrUpdate(ref afterBlink, newStones, stone.Value);
                }
                // Print(afterBlink);
                stones = afterBlink;
            }

            Console.WriteLine(stones.Sum(o => o.Value));
        }

        private void AddOrUpdate(ref Dictionary<ulong, long> afterBlink, ulong[] stones, long count)
        {
            foreach (var stone in stones)
            {
                if (afterBlink.ContainsKey(stone))
                {
                    afterBlink[stone] += count;
                }
                else
                {
                    afterBlink.Add(stone, count);
                }
            }
        }

        private ulong[] ApplyRules(ulong stone)
        {
            // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            if (stone == 0)
            {
                return [1];
            }

            // If the stone is engraved with a number that has an even number of digits,
            // it is replaced by two stones.
            // The left half of the digits are engraved on the new left stone,
            // and the right half of the digits are engraved on the new right stone.
            // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)

            var stoneNumber = stone.ToString();
            var digitCount = GetDigitCount(stone);
            if (digitCount % 2 == 0)
            {
                var newStoneLength = digitCount / 2;
                var leftHalf = ulong.Parse(stoneNumber.Substring(0, newStoneLength));
                var rightHalf = ulong.Parse(stoneNumber.Substring(newStoneLength));
                return [leftHalf, rightHalf];
            }

            // If none of the other rules apply,
            // the stone is replaced by a new stone;
            // the old stone's number multiplied by 2024 is engraved on the new stone.
            return [stone * 2024];
        }

        private void Print(List<ulong> stones)
        {
            foreach (var stone in stones)
            {
                Console.Write($"{stone} ");
            }
            Console.WriteLine();
        }

        private static int GetDigitCount(ulong number)
        {
            if (number == 0)
            {
                return 1;
            }
            return (int)Math.Floor(Math.Log10(number) + 1);
        }

        private List<ulong> GetInput(string path)
        {
            var lines = File.ReadLines(path).ToList();
            return lines[0].Split(" ").Select(ulong.Parse).ToList();
        }

        private static ulong[] GetInput2(string path)
        {
            var lines = File.ReadLines(path).ToList();
            return lines[0].Split(" ").Select(ulong.Parse).ToArray();
        }

    }
}
