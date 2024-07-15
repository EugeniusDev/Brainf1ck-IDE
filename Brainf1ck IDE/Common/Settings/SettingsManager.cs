using Brainf1ck_IDE.Common.FileProcessing;

namespace Brainf1ck_IDE.Common.Settings
{
    public static class SettingsManager
    {
        public static void SetNewDefaultFolder(string folderPath)
        {
            GlobalSettings globalSettings = StorageReader.RetrieveGlobalSettings();
            globalSettings.DefaultFolderForNewProjects = folderPath;
            StorageWriter.SaveGlobalSettings(globalSettings);
        }
    }
}
