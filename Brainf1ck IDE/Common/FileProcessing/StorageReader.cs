using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Common.Settings;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class StorageReader
    {
        public static ObservableCollection<ProjectMetadata> RetrieveAllProjectsData()
        {
            if (File.Exists(FilePaths.projectsDataList))
            {
                string jsonString = File.ReadAllText(FilePaths.projectsDataList);
                try
                {
                    var result = JsonSerializer
                        .Deserialize<ObservableCollection<ProjectMetadata>>(jsonString);
                    return result ?? [];
                }
                catch
                {
                    // Failed to parse json, so return default
                }
            }

            return [];
        }

        public static ProjectProperties? RetrieveProjectFrom(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                try
                {
                    return JsonSerializer.Deserialize<ProjectProperties>(jsonString);
                }
                catch
                {
                    // Failed to parse json, so return default
                }
            }

            return null;
        }

        public static GlobalSettings RetrieveGlobalSettings()
        {
            if (File.Exists(FilePaths.globalSettingsPath))
            {
                string jsonString = File.ReadAllText(FilePaths.globalSettingsPath);
                try
                {
                    return JsonSerializer.Deserialize<GlobalSettings>(jsonString)
                        ?? new();
                }
                catch
                {
                    // Failed to parse json, so return default
                }
            }

            return new();
        }
    }
}
