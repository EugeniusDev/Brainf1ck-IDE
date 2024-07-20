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
            projects.WriteTo(FilePaths.projectsDataList);
        }

        private static void WriteTo<T>(this T obj, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, jsonString);
        }

        public static void SaveGlobalSettings(GlobalSettings globalSettings)
        {
            globalSettings.WriteTo(FilePaths.globalSettingsPath);
        }

        public static void SaveProjectPropertiesToFile(string filePath, ProjectProperties props)
        {
            props.WriteTo(filePath);
        }

        public static void SaveFile(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }
    }
}
