using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Brainf1ck_IDE.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProjectMetadata> projects;
        [ObservableProperty]
        private ProjectProperties projectToCreate = new();

        [ObservableProperty]
        private string memoryLengthInput = string.Empty;
        [ObservableProperty]
        private string initialIndexInput = string.Empty;
        [ObservableProperty]
        private string errorMessage = string.Empty;
        public MainPageViewModel()
        {
            projects = StorageReader.RetrieveAllProjectsData()
                .CleanupFromUnexisting();
            SaveProjectsList();
        }

        [RelayCommand]
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
        async Task RemoveProject(ProjectMetadata project)
        {
            if (AppShell.Current.CurrentPage is not MainPage mainPage)
            {
                return;
            }

            bool isRemovingConfirmed = await mainPage
                .DisplayAlert("Removing project from the list",
                "Note that project files will not be deleted", 
                "Ok", "Cancel");
            if (isRemovingConfirmed)
            {
                RemoveProjectFromExisting(project);
            }
        }

        public void RemoveProjectFromExisting(ProjectMetadata project)
        {
            Projects.Remove(project);
            SaveProjectsList();
        }

        [RelayCommand]
        async Task OpenExistingProject(ProjectMetadata project)
        {
            if (AppShell.Current.CurrentPage is not MainPage mainPage)
            {
                return;
            }
            await mainPage.TryOpenExistingProject(
                StorageReader.RetrieveProjectPropertiesFrom(project.Path),
                project.Path);
        }

        [RelayCommand]
        void TryOpenNewProject()
        {
            ErrorMessage = ProjectToCreate.ParseInputWithFeedback(InitialIndexInput, 
                MemoryLengthInput);
        }
    }
}
