using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.Settings;
using Brainf1ck_IDE.Domain;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectStructurator
    {
        public static bool HasValidStructure(ProjectProperties project)
        {
            // TODO implement
            return false;
        }

        public static void CreateNewProjectStructure(GlobalSettings settings, ProjectProperties project)
        {
            if (settings.RootFolderForNewProjects is null)
            {
                return;
            }
            string projectDirectoryPath = Path
                .Combine(settings.RootFolderForNewProjects,
                project.Name);
            Directory.CreateDirectory(projectDirectoryPath);

            string projectSettingsFilePath = Path
                .Combine(projectDirectoryPath, 
                $"{FilePaths.GetProjectSettingsFilename(project.Name)}");
            project.SaveToFile(projectSettingsFilePath);

            string projectFilesDirectoryPath = Path
                .Combine(projectDirectoryPath, project.Name);
            Directory.CreateDirectory(projectFilesDirectoryPath);

            if (project.FileToRun is not null)
            {
                string helloWorldCode = BrainfuckSnippets.helloWorldSnippet;
                string mainFilePath = Path.Combine(projectFilesDirectoryPath,
                    project.FileToRun);
                StorageWriter.SaveFile(mainFilePath, helloWorldCode);
            }
        }

        public static void CreateValidStructure(ProjectMetadata project)
        {
            // TODO implement
        }
    }
}
