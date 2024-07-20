using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.Settings
{
    public static class SettingsManager
    {
        public static GlobalSettings RetrieveGlobalSettings()
        {
            return StorageReader.ReadGlobalSettings();
        }

        public static void SetNewDefaultProjectFolder(string folderPath)
        {
            GlobalSettings globalSettings = StorageReader.ReadGlobalSettings();
            globalSettings.RootFolderForNewProjects = folderPath;
            StorageWriter.SaveGlobalSettings(globalSettings);
        }

        public static void SetNewDefaultProjectFolderFromSettingsFile(string projectSettingsFilePath)
        {
            var projectFolder = Directory.GetParent(projectSettingsFilePath);
            if (projectFolder is not null
                && projectFolder.Parent is not null)
            {
                SetNewDefaultProjectFolder(projectFolder.Parent.FullName);
            }
        }
    }
}
