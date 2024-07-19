using Brainf1ck_IDE.Common;
using Brainf1ck_IDE.Common.ProjectRelated;
using System.Text;

namespace Brainf1ck_IDE.Domain
{
    public class BrainfuckErrorParser(ProjectProperties projectProperties)
    {
        private readonly ProjectProperties projectProperties = projectProperties;

        public readonly Dictionary<BrainfuckErrorTypes, string> ErrorDescriptions = new(){
            { BrainfuckErrorTypes.IndexOutOfMemoryRange,
                "Memory cell index got out of memory range" }
        };

        public bool TryRetrieveErrorsFrom(string brainfuckCode, out string errorsOutput)
        {
            // TODO implement checks on corresponding [] and <> etc

            List<string> errors = [];
            errors.Add($"Error at line {0}, symbol {0}");

            StringBuilder sb = new();
            errors.ForEach(e => sb.AppendLine(e));
            errorsOutput = sb.ToString();
            return !string.IsNullOrEmpty(errorsOutput);
        }
    }
}
