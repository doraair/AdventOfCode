﻿namespace AdventOfCode.Day05
{
    using System.Linq;

    internal class PartOne
    {
        private const string pageOrderingRulesPath = "./Day05/input-order-rules.txt";
        private const string pagesToProducePath = "./Day05/input-pages-to-produce.txt";

        private Dictionary<int, List<int>> pageOrderingRules = new Dictionary<int, List<int>>();
        internal void Run()
        {
            GetPageOrderingRules(pageOrderingRulesPath);
            var pagestoProduce = GetPageToProduce(pagesToProducePath);
            var totalMiddlePageNumber = 0;

            foreach (var page in pagestoProduce)
            {
                if (IsPageProduced(page))
                {
                    Console.WriteLine($"Yes - {string.Join(",", page)}");
                    totalMiddlePageNumber += page[page.Length / 2];
                }
            }
            Console.WriteLine($"totalMiddlePageNumber - {totalMiddlePageNumber}");
        }

        private bool IsPageProduced(int[] pagesToProduce)
        {
            var isValid = true;
            var pagesLength = pagesToProduce.Length;
            for (var i = pagesLength - 1; i >= 0; --i)
            {
                for (var j = i - 1; j >= 0; --j)
                {
                    if (!Validate(pagesToProduce[i], pagesToProduce[j]))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid)
                {
                    break;
                }
            }
            return isValid;
        }

        private bool Validate(int pageNumber, int before)
        {
            if (!pageOrderingRules.ContainsKey(pageNumber))
            {
                return true;
            }

            var rule = pageOrderingRules[pageNumber];
            return !rule.Any(o => o == before);
        }

        private void GetPageOrderingRules(string path)
        {
            var data = File.ReadLines(path)
                .Select(line => line.Split("|"))
                .Select(values => values.Select(int.Parse).ToArray())
                .Select(o => new { Key = o[0], Values = o[1] })
                .ToList();
            var d = data.GroupBy(o => o.Key).ToList();

            foreach (var item in d)
            {
                pageOrderingRules.Add(item.Key, item.Select(o => o.Values).ToList());
            }

        }

        private int[][] GetPageToProduce(string path)
        {
            return File.ReadLines(path)
                .Select(line => line.Split(","))
                .Select(values => values.Select(int.Parse).ToArray()).ToArray();
        }
    }
}
