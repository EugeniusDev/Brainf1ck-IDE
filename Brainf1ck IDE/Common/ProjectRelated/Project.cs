using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public class Project
    {
        public string Name { get; set; }
        public uint MemoryLength { get; set; }
        public string? FileToRun { get; set; }

        public Project(string name)
        {
            Name = name;
            MemoryLength = 30000;
            FileToRun = FilePaths.welcomeScriptFile;
        }

        public Project(string name, uint memoryLength, string fileToRun)
        {
            Name = name;
            MemoryLength = memoryLength;
            FileToRun = fileToRun;
        }
    }
}
