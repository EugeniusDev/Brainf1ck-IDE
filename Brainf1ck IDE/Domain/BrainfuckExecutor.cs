using Brainf1ck_IDE.Common.ProjectRelated;
using System.Text;

namespace Brainf1ck_IDE.Domain
{
    public class BrainfuckExecutor(ProjectProperties projectProperties)
    {
        private readonly ProjectProperties projectProperties = projectProperties;

        public string Execute(string bfInput)
        {
            byte[] memory = new byte[projectProperties.MemoryLength];
            uint cellIndex = projectProperties.InitialCellIndex;
            StringBuilder stringBuilder = new();
            for (int i = 0; i < bfInput.Length; i++)
            {
                switch (bfInput[i])
                {
                    case '+':
                        memory[cellIndex]++;
                        break;
                    case '-':
                        memory[cellIndex]--;
                        break;
                    case '>':
                        cellIndex++;
                        break;
                    case '<':
                        cellIndex--;
                        break;
                    case ',':
                        memory[cellIndex] = (byte)bfInput[i];
                        break;
                    case '.':
                        stringBuilder.Append((char)memory[cellIndex]);
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

            return stringBuilder.ToString();
        }

    }
}
