using Brainf1ck_IDE.Common.FileProcessing;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.Common.ProjectRelated
{
    public static class ProjectExtensionMethods
    {
        public static ObservableCollection<ProjectMetadata> CleanupFromUnexisting(this ObservableCollection<ProjectMetadata> projects)
        {
            for (ushort i = 0; i < projects.Count; i++)
            {
                string projectName = projects[i].Name;
                if (!File.Exists(
                    FilePaths.GetProjectSettingsFilename(projectName)))
                {
                    projects.RemoveAt(i);
                    i--;
                }
            }
            return projects.Distinct();
        }

        public static ObservableCollection<ProjectMetadata> Distinct(this ObservableCollection<ProjectMetadata> projects)
        {
            // TODO implement distinct
            return projects;
        }

        public static void SaveProjectsData(this ObservableCollection<ProjectMetadata> projects)
        {
            StorageWriter.SaveProjectsData(projects);
        }
    }
}
