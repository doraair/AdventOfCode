namespace AdventOfCode.Day07
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    internal class PartTwo
    {
        private const string inputPath = "./Day07/input.txt";
        private List<EquationResults> equationResults = new List<EquationResults>();

        internal void Run()
        {
            var equations = GetInput(inputPath);

            ulong sumOfTestValues = 0;
            foreach (var equation in equations)
            {
                var count = equation.Numbers.Count();
                if (CanMatchTestResult(equation.Numbers, equation.TestResult))
                {
                    //Console.WriteLine($"The numbers can be combined to match the test result {equation.TestResult}");
                    sumOfTestValues += equation.TestResult;
                }
                else
                {
                    var combindNumber = ulong.Parse($"{equation.Numbers[0]}{equation.Numbers[1]}");
                    if (CanMatchTestResultRecursive2(equation.Numbers, equation.TestResult, 0, equation.Numbers[0], equation.Numbers[0].ToString()))
                    {
                        //Console.WriteLine($"The numbers can be combined to match the test result {equation.TestResult}");
                        //Console.WriteLine($"{equation.TestResult} = {string.Join(" ", equation.Numbers)}");
                        sumOfTestValues += equation.TestResult;
                    }
                    else
                    {
                        Console.WriteLine($"{equation.TestResult} = {string.Join(" ", equation.Numbers)}");
                    }
                }
                Console.WriteLine();
            }
            //PrintEquationResults();
            Console.WriteLine($"{sumOfTestValues}");
        }

        private bool CanMatchTestResult(ulong[] numbers, ulong testResult)
        {
            return CanMatchTestResultRecursive(numbers, testResult, 0, numbers[0]);
        }

        private bool CanMatchTestResultRecursive(ulong[] numbers, ulong testResult, int index, ulong currentResult)
        {
            if (index == numbers.Length - 1)
            {
                return currentResult == testResult;
            }

            var nextIndex = index + 1;
            var nextNumber = numbers[nextIndex];

            return CanMatchTestResultRecursive(numbers, testResult, nextIndex, currentResult + nextNumber) ||
                   CanMatchTestResultRecursive(numbers, testResult, nextIndex, currentResult * nextNumber);
        }
        private bool CanMatchTestResultRecursive2(ulong[] numbers, ulong testResult, int index, ulong currentResult, string equation = "")
        {
            if (index >= numbers.Length - 1)
            {
                var result = currentResult == testResult;
                equationResults.Add(new EquationResults { Success = result, Equation = $"{equation} = > {currentResult}", TestResult = testResult });
                return currentResult == testResult;
            }

            var nextIndex = index + 1;
            var nextNumber = numbers[nextIndex];

            // Combine the current result and next number using the || operator
            var combinedNumber = ulong.Parse($"{currentResult}{nextNumber}");
            ulong nextNumberForCombined = 0;
            if (nextIndex == numbers.Length - 1)
            {
                nextNumberForCombined = 0;
            }
            else
            {
                nextNumberForCombined = numbers[nextIndex + 1];
            }
            return CanMatchTestResultRecursive2(numbers, testResult, nextIndex, currentResult + nextNumber, $"{equation} + {nextNumber}") ||
             CanMatchTestResultRecursive2(numbers, testResult, nextIndex, currentResult * nextNumber, $"{equation} * {nextNumber}") ||
             CanMatchTestResultRecursive2(numbers, testResult, nextIndex, combinedNumber, $"{equation} || {nextNumber}");
        }

        private void PrintEquationResults()
        {
            equationResults = equationResults.OrderBy(o => o.TestResult).ToList();
            foreach (var item in equationResults)
            {
                Console.WriteLine($"{item.Success} - {item.TestResult}: {item.Equation}");
            }
        }

        private List<LineModel> GetInput(string path)
        {
            return File.ReadLines(path)
                .Select(o => o.Split(':'))
                .Select(o => new LineModel { TestResult = ulong.Parse(o[0]), Numbers = o[1].Trim().Split(" ").Select(ulong.Parse).ToArray() })
                .ToList();
        }
    }
}
