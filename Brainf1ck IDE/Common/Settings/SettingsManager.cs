using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.Settings
{
    public static class SettingsManager
    {
        public static GlobalSettings RetrieveGlobalSettings()
        {
            return StorageReader.RetrieveGlobalSettings();
        }

        public static void SetNewDefaultFolder(string folderPath)
        {
            GlobalSettings globalSettings = StorageReader.RetrieveGlobalSettings();
            globalSettings.RootFolderForNewProjects = folderPath;
            StorageWriter.SaveGlobalSettings(globalSettings);
        }
    }
}
