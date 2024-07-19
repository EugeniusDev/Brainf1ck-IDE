using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public class ProjectProperties
    {
        public string Name { get; set; } = string.Empty;
        public uint MemoryLength { get; set; } = 30000;
        public uint InitialCellIndex { get; set; } = 0;

        public bool AreNumeralPropertiesValid()
        {
            return InitialCellIndex >= 0
                && MemoryLength >= 30000
                && MemoryLength > InitialCellIndex;
        }
    }
}
