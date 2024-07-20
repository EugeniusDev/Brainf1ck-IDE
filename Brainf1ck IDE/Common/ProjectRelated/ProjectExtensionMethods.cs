using Brainf1ck_IDE.Common.FileProcessing;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectExtensionMethods
    {
        public static ObservableCollection<ProjectMetadata> CleanupFromUnexisting(this ObservableCollection<ProjectMetadata> dirtyProjects)
        {
            ObservableCollection<ProjectMetadata> existingProjects = [];
            for (ushort i = 0; i < dirtyProjects.Count; i++)
            {
                if (File.Exists(dirtyProjects[i].Path))
                {
                    existingProjects.Add(dirtyProjects[i]);
                }
            }

            return existingProjects.Distinct();
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

        public static async Task OpenInProjectPage(this ProjectProperties project)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ProjectPage)}?{nameof(ProjectProperties)}={project}",
                true,
                new Dictionary<string, object>
                {
                    {nameof(ProjectProperties), project}
                });
        }
    }
}
