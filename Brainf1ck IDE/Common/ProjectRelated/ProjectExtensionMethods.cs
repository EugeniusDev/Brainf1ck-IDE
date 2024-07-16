using Brainf1ck_IDE.Common.FileProcessing;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectExtensionMethods
    {
        public static ObservableCollection<ProjectMetadata> CleanupFromUnexisting(this ObservableCollection<ProjectMetadata> dirtyProjects)
        {
            ObservableCollection<ProjectMetadata> cleanProjectsData = [];
            for (ushort i = 0; i < dirtyProjects.Count; i++)
            {
                string projectName = dirtyProjects[i].Name;
                if (File.Exists(
                    FilePaths.GetProjectSettingsFilename(projectName)))
                {
                    cleanProjectsData.Add(dirtyProjects[i]);
                }
            }

            return dirtyProjects.Distinct();
        }

        public static ObservableCollection<ProjectMetadata> Distinct(this ObservableCollection<ProjectMetadata> projects)
        {
            var distinctData = projects.Distinct<ProjectMetadata>();
            return [.. distinctData];
        }

        public static void SaveProjectsData(this ObservableCollection<ProjectMetadata> projects)
        {
            StorageWriter.SaveProjectsData(projects);
        }

        public static void SaveToFile(this ProjectProperties props, string filePath)
        {
            StorageWriter.SaveProjectPropertiesToFile(filePath, props);
        }
    }
}
