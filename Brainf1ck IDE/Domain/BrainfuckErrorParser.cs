using Brainf1ck_IDE.Common;
using Brainf1ck_IDE.Common.ProjectRelated;
using System.Text;

namespace Brainf1ck_IDE.Domain
{
    public class BrainfuckErrorParser(ProjectProperties projectProperties)
    {
        private readonly ProjectProperties projectProperties = projectProperties;
        private const string messageAboutInfo = "Please, press \"Help\" button for more info";
        private static readonly char[] splitCharacters = { '\n', '\r' };
        private static readonly Dictionary<BrainfuckErrorTypes, string> errorMessages = new(){
            { BrainfuckErrorTypes.IndexOutOfMemoryRange,
                "Memory cell index got out of memory range" },
            { BrainfuckErrorTypes.NotBalancedSquareBrackets, 
                "[] (square brackets) are not balanced. You forgot to open/close loop" },
            { BrainfuckErrorTypes.NoParsableSymbolsFound, 
                "Your code contains no symbols that form Brainfuck programming language" },
            { BrainfuckErrorTypes.NoInput, 
                "No input given: no symbol found before , (comma)" },
        };

        public bool TryRetrieveErrorsFrom(string brainfuckCode, out string output)
        {
            string[] codeInLines = BreakCodeIntoLines(brainfuckCode);
            List<BrainfuckErrorInfo> errorsInfo = [
                CheckForIndexError(codeInLines),
                CheckForSquareBalanceError(codeInLines),
                CheckForNoParsablesError(brainfuckCode),
                CheckForNoInputError(codeInLines)
            ];

            output = FormErrorsOutput(errorsInfo);
            return !string.IsNullOrEmpty(output);
        }

        private static string[] BreakCodeIntoLines(string brainfuckCode)
        {
            return brainfuckCode.Split(splitCharacters, 
                StringSplitOptions.RemoveEmptyEntries);
        }
        private static string FormErrorsOutput(List<BrainfuckErrorInfo> errors)
        {
            StringBuilder errorsOutput = new(string.Empty);
            bool codeContainsErrors = false;
            foreach (var error in errors)
            {
                if (!error.ErrorType.Equals(BrainfuckErrorTypes.NoError))
                {
                    codeContainsErrors = true;
                    errorsOutput.Append($"{errorMessages[error.ErrorType]}.");
                    if (!error.ErrorType.Equals(BrainfuckErrorTypes.NoParsableSymbolsFound))
                    {
                        errorsOutput.Append($" Error found at line {error.Line}.");
                    }
                    errorsOutput.Append('\n');
                }
            }
            
            if (codeContainsErrors)
            {
                errorsOutput.AppendLine(messageAboutInfo);
            }

            return errorsOutput.ToString();
        }

        #region Checks
        private BrainfuckErrorInfo CheckForIndexError(string[] codeInLines)
        {
            uint currentCellIndex = projectProperties.InitialCellIndex;
            for (uint line = 0; line < codeInLines.Length; line++)
            {
                foreach (char symbol in codeInLines[line])
                {
                    if (symbol.Equals('<'))
                    {
                        currentCellIndex--;
                    }
                    else if (symbol.Equals('>'))
                    {
                        currentCellIndex++;
                    }

                    if (currentCellIndex >= projectProperties.MemoryLength
                        || currentCellIndex < 0)
                    {
                        return new BrainfuckErrorInfo(line,
                            BrainfuckErrorTypes.IndexOutOfMemoryRange);
                    }
                }
            }
            return new();
        }

        private static BrainfuckErrorInfo CheckForSquareBalanceError(string[] codeInLines)
        {
            int bracketCounter = 0;
            for (uint line = 0; line < codeInLines.Length; line++)
            {
                string code = codeInLines[line];
                foreach (char ch in code)
                {
                    if (ch.Equals('['))
                    {
                        bracketCounter++;
                    }
                    else if (ch.Equals(']'))
                    {
                        bracketCounter--;
                    }

                    // Too much ]
                    if (bracketCounter < 0)
                    {
                        return new(line, BrainfuckErrorTypes.NotBalancedSquareBrackets);
                    }
                }
            }

            // Too much [
            if (bracketCounter != 0)
            {
                return new((uint)(codeInLines.Length - 1), BrainfuckErrorTypes.NotBalancedSquareBrackets);
            }

            return new();
        }

        private static BrainfuckErrorInfo CheckForNoParsablesError(string code)
        {
            if (code.Contains('+') || code.Contains('-')
                || code.Contains('<') || code.Contains('>')
                || code.Contains('[') || code.Contains(']')
                || code.Contains('.') || code.Contains(','))
            {
                return new();
            }

            return new(0, BrainfuckErrorTypes.NoParsableSymbolsFound);
        }

        private static BrainfuckErrorInfo CheckForNoInputError(string[] codeInLines)
        {
            for (uint line = 0; line < codeInLines.Length; line++)
            {
                if (codeInLines[line].Contains(','))
                {
                    int indexOfFirstDot = codeInLines[line].IndexOf(',');
                    if (indexOfFirstDot == 0)
                    {
                        return new(line, BrainfuckErrorTypes.NoInput);
                    }
                }
            }

            return new();
        }
        #endregion
    }
}
