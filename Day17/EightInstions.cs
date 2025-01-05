namespace AdventOfCode.Day17
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class EightInstructions
    {
        private long registerA = 0;
        private long registerB = 0;
        private long registerC = 0;

        private List<short> outputs = new List<short>();

        private delegate void OpcodeProcess(short operand);
        private Dictionary<int, OpcodeProcess> opcodeProcess;
        private bool isJumped = false;

        internal EightInstructions(
            long registerA,
            long registerB,
            long registerC)
        {
            this.registerA = registerA;
            this.registerB = registerB;
            this.registerC = registerC;
            opcodeProcess = new Dictionary<int, OpcodeProcess> {
                {0, AdvProcess},
                {1, BxlProcess},
                {2, BstProcess},
                {3, JnzProcess},
                {4, BxcProcess},
                {5, OutProcess},
                {6, BdvProcess},
                {7, CdvProcess},
            };
        }

        internal string RunProgram(short[] command)
        {
            var pointer = 0;
            while (pointer < command.Length)
            {
                if (pointer + 1 >= command.Length)
                {
                    break;
                }
                var opcode = command[pointer];
                var operand = command[pointer + 1];
                opcodeProcess[opcode](operand);

                if (isJumped)
                {
                    pointer = operand;
                    isJumped = false;
                }
                else
                {
                    pointer += 2;
                }
            }

            return string.Join(",", outputs);
        }


        private string expectedOutputString = "2,4,1,5,7,5,1,6,0,3,4,6,5,5,3,0";
        private short[] expectedOutput;
        internal void Reverse()
        {
            // program
            // 2,4,1,5,7,5,1,6,0,3,4,6,5,5,3,0

            // oupput
            // 3,6,3,7,0,7,0,3,0.

            // registerA0 = {0,1,2,3,4,5} probability 0-5
            // registerA0 = (registerA0 * 6 ) - (registerA0 * 6 + {0-5})


            expectedOutput = expectedOutputString.Split(',').Select(short.Parse).ToArray();
            FindingThePreviousA(0, expectedOutput.Length - 1);
            Console.WriteLine(registerA);
        }

        private void FindingThePreviousA(long a, int expectedOutputIndex)
        {
            if (expectedOutputIndex == -1)
            {
                registerA = a;
                return;
            }

            for (long prop = 0; prop < 6; prop++)
            {
                var previousA = a * 6 + prop;
                // test
                registerA = previousA;
                var programOutput = RunHardCodeProgram();

                // assert: check the last output
                if (expectedOutputString.EndsWith(programOutput))
                {
                    FindingThePreviousA(previousA, expectedOutputIndex - 1);
                }
            }
        }

        internal string RunHardCodeProgram()
        {
            // program
            // 2,4,1,5,7,5,1,6,0,3,4,6,5,5,3,0

            // oupput
            // 3,6,3,7,0,7,0,3,0.

            // registerA0 = {0,1,2,3,4,5} probability 0-5
            // registerA0 = (registerA0 * 6 ) - (registerA0 * 6 + {0-5})
            registerB = 0;
            registerC = 0;
            outputs.Clear();
            do
            {
                // 2,4,
                registerB = registerA % 8;

                // 1,5,
                registerB = registerB ^ 5;

                // 7,5,
                registerC = registerA / (long)Math.Pow(2, registerB);

                // 1,6,
                registerB = registerB ^ 6;

                // 0,3,
                registerA = registerA / 6;

                // 4,6,
                registerB = registerC ^ registerB;

                // 5,5,
                var output = registerB % 8;
                outputs.Add((short)output);
                // 3,0
            } while (registerA != 0);

            return string.Join(",", outputs);
        }

        private void AdvProcess(short operand)
        {
            registerA = AdvDivision(operand);
        }

        private int AdvDivision(short operand)
        {
            var input = GetOperandValue(operand);

            var divideBy = Math.Pow(2, input);
            var result = registerA / divideBy;
            return (int)Math.Truncate(result);
        }

        private void BxlProcess(short operand)
        {
            // XOR
            registerB = registerB ^ operand;
        }

        private void BstProcess(short operand)
        {
            var input = GetOperandValue(operand);

            // % mod
            // (thereby keeping only its lowest 3 bits)
            registerB = input % 8;
        }

        private void JnzProcess(short operand)
        {
            var input = GetOperandValue(operand);
            if (registerA == 0)
            {
                return;
            }
            isJumped = true;
            // it jumps by setting the instruction pointer to the value of its literal operand; 
            // if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
        }
#pragma warning disable IDE0060
        private void BxcProcess(short operand)
        {
            // ignore operand
            // Do XOR
            registerB = registerC ^ registerB;
        }
#pragma warning restore IDE0060

        private void OutProcess(short operand)
        {
            var input = GetOperandValue(operand);
            var output = input % 8;
            outputs.Add((short)output);
        }

        private void BdvProcess(short operand)
        {
            registerB = AdvDivision(operand);
        }

        private void CdvProcess(short operand)
        {
            registerC = AdvDivision(operand);
        }

        private long GetOperandValue(short operand)
        {
            switch (operand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 7:
                    return operand;
                case 4:
                    return registerA;
                case 5:
                    return registerB;
                case 6:
                    return registerC;
                default:
                    throw new InvalidOperationException();
            }

        }
    }
}
