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

        public static string ParseInputWithFeedback(this ProjectProperties project,
            string indexString, string memoryLengthString)
        {
            if (string.IsNullOrWhiteSpace(indexString)
                || string.IsNullOrWhiteSpace(memoryLengthString))
            {
                return "Lack of input. " +
                    "Please fill all required fields";
            }

            if (uint.TryParse(indexString, out uint index)
                && uint.TryParse(memoryLengthString, out uint memoryLength))
            {
                project.InitialCellIndex = index;
                project.MemoryLength = memoryLength;
            }
            else
            {
                return "Wrong input. " +
                    "Memory length and initial index must be positive integer numbers";
            }

            if (!FileValidator.IsStringValidForFilesystem(project.Name))
            {
                return "Invalid project name. " +
                    "Please, use only English literals, digits and underscores";
            }
            if (!project.AreNumeralPropertiesValid())
            {
                return "Invalid project settings. " +
                    "Memory length should be at least 30000. " +
                    "In this case index can vary from 0 to 29999";
            }

            return string.Empty;
        }
    }
}
