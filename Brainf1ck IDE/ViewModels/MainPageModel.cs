using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.ViewModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<ProjectMetadata> projects;
        public MainPageModel()
        {
            projects = StorageReader.RetrieveAllProjectsData()
                .CleanupFromUnexisting();
            SaveProjectsList();
        }

        [RelayCommand]
        void TryOpenProject(Project project)
        {
            // TODO make extension methods for Project (check if valid and then open)
            // if not valid, autoremove with alert
        }

        [RelayCommand]
        void AddProject(ProjectMetadata project)
        {
            AppendNewProjectToExisting(project);
        }

        public void AppendNewProjectToExisting(ProjectMetadata project)
        {
            if (!Projects.Contains(project))
            {
                Projects.Add(project);
                SaveProjectsList();
            }
        }

        public void SaveProjectsList()
        {
            Projects.SaveProjectsData();
        }

        [RelayCommand]
        void RemoveProject(ProjectMetadata project)
        {
            RemoveProjectFromExisting(project);
        }

        public void RemoveProjectFromExisting(ProjectMetadata project)
        {
            Projects.Remove(project);
            SaveProjectsList();
        }
    }
}
