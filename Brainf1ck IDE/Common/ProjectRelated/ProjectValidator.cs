using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.Settings;
using Brainf1ck_IDE.Domain;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectValidator
    {
        private static readonly Dictionary<ProjectInvalidityTypes, string> invalidityMessages = new()
        {
            { ProjectInvalidityTypes.InputLack,  "Lack of input. " +
                "Please fill all required fields" },
            { ProjectInvalidityTypes.WrongInput, "Wrong input. " +
                "Memory length and initial index must be positive integer numbers" },
            { ProjectInvalidityTypes.InvalidName, "Invalid project name. " +
                "Please, use only English literals, digits and underscores"},
            { ProjectInvalidityTypes.InvalidProperties, "Invalid project settings. " +
                "Memory length should be at least 30000. " +
                "In this case index can vary from 0 to 29999" }
        };

        private static string FormProjectFolderPath(ProjectProperties project)
        {
            if (IsRootFolderSet())
            {
                GlobalSettings settings = SettingsManager.RetrieveGlobalSettings();
                return Path.Combine(settings.RootFolderForNewProjects!,
                    project.Name);
            }

            return string.Empty;
        }

        private static bool IsRootFolderSet()
        {
            GlobalSettings settings = StorageReader.ReadGlobalSettings();
            return settings.RootFolderForNewProjects is not null;
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
            if (IsRootFolderSet())
            {
                return Path.Combine(FormProjectFolderPath(project),
                    FilePaths.GetProjectSettingsFilename(project.Name));
            }
            
            return string.Empty;
        }

        public static void EnsureCorrectStructure(ProjectProperties project)
        {
            CreateRequiredFolders(project);
            project.SaveToFile(FormProjectSettingsPath(project));            
        }

        private static void CreateRequiredFolders(ProjectProperties project)
        {
            if (IsRootFolderSet())
            {
                string projectFilesFolder = FormProjectFilesFolderPath(project);
                Directory.CreateDirectory(FormProjectFolderPath(project));
                Directory.CreateDirectory(projectFilesFolder);
            }
        }

        public static void CreateWelcomeFileFor(ProjectProperties project)
        {
            string mainFilePath = Path.Combine(FormProjectFilesFolderPath(project),
                FilePaths.welcomeScriptFileName);
            string helloWorldCode = BrainfuckSnippets
                .GetMarkedSnippetByKey(CodeSnippets.HelloWorld);
            StorageWriter.SaveFile(mainFilePath, helloWorldCode);
        }

        public static string TryPopulateAndGiveFeedback(this ProjectProperties project,
            string indexString, string memoryLengthString)
        {
            if (string.IsNullOrWhiteSpace(indexString)
                || string.IsNullOrWhiteSpace(memoryLengthString))
            {
                return invalidityMessages[ProjectInvalidityTypes.InputLack];
            }

            if (uint.TryParse(indexString, out uint index)
                && uint.TryParse(memoryLengthString, out uint memoryLength))
            {
                project.InitialCellIndex = index;
                project.MemoryLength = memoryLength;
            }
            else
            {
                return invalidityMessages[ProjectInvalidityTypes.WrongInput];
            }

            if (!FileValidator.IsStringValidForFilesystem(project.Name))
            {
                return invalidityMessages[ProjectInvalidityTypes.InvalidName];
            }
            if (!project.AreNumeralPropertiesValid())
            {
                return invalidityMessages[ProjectInvalidityTypes.InvalidProperties];
            }

            return string.Empty;
        }

    }
}
