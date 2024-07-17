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
                "Ok", "Cancel", FlowDirection.LeftToRight);
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
            ErrorMessage = string.Empty;
            if (uint.TryParse(InitialIndexInput, out uint index)
                && uint.TryParse(MemoryLengthInput, out uint memoryLength))
            {
                ProjectToCreate.InitialCellIndex = index;
                ProjectToCreate.MemoryLength = memoryLength;
            }
            else if (!string.IsNullOrWhiteSpace(InitialIndexInput)
                || !string.IsNullOrWhiteSpace(MemoryLengthInput))
            {
                ErrorMessage = "Wrong input. " +
                    "Memory length and initial index should be positive integer numbers";
            }

            if (!IsProjectNameValid(ProjectToCreate.Name))
            {
                ErrorMessage = "Invalid project name. " +
                    "Please, use only English literals, digits and underscores";
            }
            if (!AreProjectNumeralPropertiesValid(ProjectToCreate))
            {
                ErrorMessage = "Invalid project settings. " +
                    "Memory length should be at least 30000. " +
                    "In this case index can vary from 0 to 29999";
            }
        }

        private static bool IsProjectNameValid(string projectName)
        {
            Regex regex = new("^[a-zA-Z0-9_]+$");
            return !string.IsNullOrWhiteSpace(projectName) 
                && regex.IsMatch(projectName);
        }
        private static bool AreProjectNumeralPropertiesValid(ProjectProperties project)
        {
            return project.InitialCellIndex >= 0
                && project.MemoryLength >= 30000
                && project.MemoryLength > project.InitialCellIndex;
        }
    }
}
