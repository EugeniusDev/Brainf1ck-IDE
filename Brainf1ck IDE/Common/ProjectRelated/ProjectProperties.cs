using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public class ProjectProperties
    {
        public string Name { get; set; } = string.Empty;
        public uint MemoryLength { get; set; } = 30000;
        public uint InitialCellIndex { get; set; } = 0;
        public string? FileToRun { get; set; } = FilePaths.welcomeScriptFile;

        public void SetBackDefaultFileToRun()
        {
            FileToRun = FilePaths.welcomeScriptFile;
        }
    }
}
