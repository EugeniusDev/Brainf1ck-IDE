using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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

        private MainPage mainPage = (MainPage)AppShell.Current.CurrentPage;
        public MainPageViewModel()
        {
            projects = StorageReader.ReadAllProjectsData()
                .CleanupFromUnexisting();
            SaveProjectsList();
        }


        private void SaveProjectsList()
        {
            Projects.SaveProjectsData();
        }

        public void BindToMainPage(MainPage mainPage)
        {
            this.mainPage = mainPage;
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

        [RelayCommand]
        async Task RemoveProject(ProjectMetadata project)
        {
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
            await mainPage.TryOpenProject(
                StorageReader.ReadProjectPropertiesFrom(project.Path),
                project.Path);
        }

        [RelayCommand]
        async Task TryOpenNewProject()
        {
            ErrorMessage = ProjectToCreate.TryPopulateAndGiveFeedback(InitialIndexInput, 
                MemoryLengthInput);
            await mainPage.TryFinalizeProjectCreation(ProjectToCreate);
        }
    }
}
