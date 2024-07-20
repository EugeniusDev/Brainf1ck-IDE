using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Common.Settings;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class StorageReader
    {
        public static ObservableCollection<ProjectMetadata> ReadAllProjectsData()
        {
            string jsonString = TryReadFileContent(FilePaths.projectsDataList);
            try
            {
                var result = JsonSerializer
                    .Deserialize<ObservableCollection<ProjectMetadata>>(jsonString);
                return result ?? [];
            }
            catch
            {
                // Failed to parse json, so return empty
            }

            return [];
        }

        public static string TryReadFileContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return string.Empty;
        }

        public static ProjectProperties? ReadProjectPropertiesFrom(string filePath)
        {
            string jsonString = TryReadFileContent(filePath);
            return jsonString.TryDeserializeInObject<ProjectProperties>();
        }

        private static T? TryDeserializeInObject<T>(this string jsonString) where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch
            {
                return null;
            }
        }

        public static GlobalSettings ReadGlobalSettings()
        {
            string jsonString = TryReadFileContent(FilePaths.globalSettingsPath);
            return jsonString.TryDeserializeInObject<GlobalSettings>()
                ?? new();
        }

        public static BrainfuckFile ReadBrainfuckFile(string filePath)
        {
            return new BrainfuckFile
            {
                Name = Path.GetFileName(filePath),
                Contents = TryReadFileContent(filePath)
            };
        }
    }
}
