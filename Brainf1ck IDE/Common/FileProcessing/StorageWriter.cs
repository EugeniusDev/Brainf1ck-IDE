using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Common.Settings;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class StorageWriter
    {
        public static void SaveProjectsData(ObservableCollection<ProjectMetadata> projects)
        {
            string jsonString = JsonSerializer.Serialize(projects);
            File.WriteAllText(FilePaths.projectsDataList, jsonString);
        }

        public static void SaveGlobalSettings(GlobalSettings globalSettings)
        {
            string jsonString = JsonSerializer.Serialize(globalSettings);
            File.WriteAllText(FilePaths.globalSettingsPath, jsonString);
        }
    }
}
