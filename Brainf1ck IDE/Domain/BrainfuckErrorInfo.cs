using Brainf1ck_IDE.Common;

namespace Brainf1ck_IDE.Domain
{
    public class BrainfuckErrorInfo
    {
        public uint Line { get; set; }
        public BrainfuckErrorTypes ErrorType { get; set; }

        public BrainfuckErrorInfo()
        {
            Line = 0;
            ErrorType = BrainfuckErrorTypes.NoError;
        }

        public BrainfuckErrorInfo(uint lineIndex, BrainfuckErrorTypes errorType)
        {
            Line = GetOrdinalLineNumber(lineIndex);
            ErrorType = errorType;
        }

        private static uint GetOrdinalLineNumber(uint lineIndex)
        {
            return lineIndex + 1;
        }
    }
}
