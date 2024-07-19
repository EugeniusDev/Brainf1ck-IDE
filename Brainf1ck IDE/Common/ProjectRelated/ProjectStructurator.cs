using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.Settings;
using Brainf1ck_IDE.Domain;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectStructurator
    {
        public static bool HasValidStructure(ProjectProperties project)
        {
            bool runnableFileIsFine = project.TargetFileName is null
                || File.Exists(project.TargetFileName);
            return Path.Exists(FormProjectFolderPath(project))
                && Path.Exists(FormProjectSettingsPath(project))
                && Path.Exists(FormProjectFilesFolderPath(project))
                && runnableFileIsFine;
        }

        public static void CreateNewProjectStructure(ProjectProperties project)
        {
            CreateFoldersAndMainFile(project);
            project.SaveToFile(FormProjectSettingsPath(project));            
        }

        private static void CreateFoldersAndMainFile(ProjectProperties project)
        {
            GlobalSettings settings = StorageReader.RetrieveGlobalSettings();
            if (settings.RootFolderForNewProjects is null)
            {
                return;
            }

            string projectFilesFolder = FormProjectFilesFolderPath(project);
            Directory.CreateDirectory(FormProjectFolderPath(project));
            Directory.CreateDirectory(projectFilesFolder);
            if (project.TargetFileName is not null)
            {
                string mainFilePath = Path.Combine(projectFilesFolder,
                    project.TargetFileName);
                if (!File.Exists(mainFilePath))
                {
                    string helloWorldCode = BrainfuckSnippets.helloWorldSnippet;
                    StorageWriter.SaveFile(mainFilePath, helloWorldCode);
                }
            }
        }

        public static string FormProjectFolderPath(ProjectProperties project)
        {
            GlobalSettings settings = StorageReader.RetrieveGlobalSettings();
            if (settings.RootFolderForNewProjects is null)
            {
                return string.Empty;
            }

            return Path.Combine(settings.RootFolderForNewProjects,
                project.Name);
        }
        public static string FormProjectFilesFolderPath(ProjectProperties project)
        {
            string projectFolder = FormProjectFolderPath(project);
            if (string.IsNullOrEmpty(projectFolder))
            {
                return string.Empty;
            }

            return Path.Combine(projectFolder, project.Name);
        }
        public static string FormProjectSettingsPath(ProjectProperties project)
        {
            GlobalSettings settings = StorageReader.RetrieveGlobalSettings();
            if (settings.RootFolderForNewProjects is null)
            {
                return string.Empty;
            }

            return Path.Combine(FormProjectFolderPath(project),
                FilePaths.GetProjectSettingsFilename(project.Name));
        }

        public static void RestoreValidStructure(ProjectProperties project)
        {
            CreateFoldersAndMainFile(project);
        }
    }
}
