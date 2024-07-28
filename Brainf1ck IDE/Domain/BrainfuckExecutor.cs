using Brainf1ck_IDE.Common.ProjectRelated;
using System.Text;

namespace Brainf1ck_IDE.Domain
{
    public class BrainfuckExecutor(ProjectProperties projectProperties)
    {
        private readonly ProjectProperties projectProperties = projectProperties;

        public Queue<ExecutionStepInfo> Execute(string bfInput, out string output)
        {
            Queue<ExecutionStepInfo> executionSteps = [];
            byte[] memory = new byte[projectProperties.MemoryLength];
            uint cellIndex = projectProperties.InitialCellIndex;
            StringBuilder stringBuilder = new();
            for (int i = 0; i < bfInput.Length; i++)
            {
                switch (bfInput[i])
                {
                    case '+':
                        memory[cellIndex]++;
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = "+",
                            Output = string.Empty,
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case '-':
                        memory[cellIndex]--;
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = "-",
                            Output = string.Empty,
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case '>':
                        cellIndex++;
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = ">",
                            Output = string.Empty,
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case '<':
                        cellIndex--;
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = "<",
                            Output = string.Empty,
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case ',':
                        memory[cellIndex] = (byte)bfInput[i - 1];
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = bfInput[i - 1] + ",",
                            Output = string.Empty,
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case '.':
                        char tempOutput = (char)memory[cellIndex];
                        stringBuilder.Append(tempOutput);
                        executionSteps.Enqueue(new ExecutionStepInfo
                        {
                            Input = ".",
                            Output = tempOutput.ToString(),
                            CellIndex = cellIndex.ToString(),
                            CellValue = memory[cellIndex].ToString()
                        });
                        break;
                    case '[':
                        // If the current cell is zero, we need to skip the loop.
                        if (memory[cellIndex] == 0)
                        {
                            int bracketCounter = 1;
                            while (bracketCounter > 0)
                            {
                                ++i;
                                if (bfInput[i] == '[')
                                {
                                    ++bracketCounter; // Found another '[', increment counter.
                                }
                                else if (bfInput[i] == ']')
                                {
                                    --bracketCounter; // Found a matching ']', decrement counter.
                                }
                            }
                        }
                        break;
                    case ']':
                        // If the current cell is not zero, we need to go back to the matching '['.
                        if (memory[cellIndex] != 0)
                        {
                            int bracketCounter = 1;
                            while (bracketCounter > 0)
                            {
                                --i;
                                if (bfInput[i] == '[')
                                {
                                    --bracketCounter; // Found the matching '[', decrement counter.
                                }
                                else if (bfInput[i] == ']')
                                {
                                    ++bracketCounter; // Found another ']', increment counter.
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            output = stringBuilder.ToString();
            return executionSteps;
        }

    }
}
