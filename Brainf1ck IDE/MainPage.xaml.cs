using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Common.Settings;
using Brainf1ck_IDE.ViewModels;
using CommunityToolkit.Maui.Storage;

namespace Brainf1ck_IDE
{
    public partial class MainPage : ContentPage
    {
        private GlobalSettings globalSettings = SettingsManager.RetrieveGlobalSettings();
        private bool shouldCreateWelcomeFileInNewProject = true;
        public MainPage(MainPageViewModel mainVm)
        {
            InitializeComponent();
            BindingContext = mainVm;
        }

        private async void CreateProjectBtn_Tapped(object sender, TappedEventArgs e)
        {
            Title = "Create new Brainfuck project";
            await MainMenuGrid.ScaleTo(.6, 100);
            MainMenuGrid.IsEnabled = false;
            MainMenuGrid.IsVisible = false;

            NewProjectGrid.IsEnabled = true;
            NewProjectGrid.IsVisible = true;
            await NewProjectGrid.ScaleTo(1d, 100);

            ProjectNameEntry.Focus();
            if (globalSettings.RootFolderForNewProjects is not null)
            {
                RootFolderLabel.Text = globalSettings.RootFolderForNewProjects;
            }
        }

        private async void OpenProjectBtn_Tapped(object sender, TappedEventArgs e)
        {
            var chosenFile = await FilePicker.PickAsync(
                new PickOptions{
                    PickerTitle = $"Please select file with " +
                    $"\"{FilePaths.ideRelatedFileExtension}\" extension",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        // TODO check if any issues appear because of ideRelatedFileExtension returns extension with dot
                        {DevicePlatform.Android, new[] { $"{FilePaths.ideRelatedFileExtension}" } },
                        {DevicePlatform.WinUI, new[] { $"{FilePaths.ideRelatedFileExtension}" } }
                    })
                });
            if (chosenFile is null)
            {
                await DisplayAlert("Selection error", 
                    "Project file was not selected", "Ok");
                return;
            }

            ProjectProperties? projectToOpen = StorageReader.RetrieveProjectPropertiesFrom(chosenFile.FullPath);
            if (projectToOpen is null)
            {
                await DisplayAlert("Selection error",
                    "Project file data is corrupt", "Ok");
                return;
            }

            ProjectMetadata newExistingProject = new()
            {
                Name = projectToOpen.Name,
                Path = chosenFile.FullPath
            };
            if (BindingContext is MainPageViewModel viewModel)
            {
                viewModel.AppendNewProjectToExisting(newExistingProject);
            }

            await TryOpenExistingProject(projectToOpen, newExistingProject.Path);
        }

        public async Task TryOpenExistingProject(ProjectProperties? project, string projectPropsFilePath)
        {
            if (project is null)
            {
                await DisplayAlert("Project data corrupt", "Can not open project", "Ok");
                return;
            }

            SettingsManager
                .SetNewDefaultProjectFolderFromSettingsFile(projectPropsFilePath);
            if (!ProjectStructurator.HasValidStructure(project))
            {
                await DisplayAlert("Corrupt project structure",
                    "Looks like project\'s structure have changed. Trying to restore it",
                    "Ok");
                ProjectStructurator.RestoreValidStructure(project);
            }

            await NavigateToProjectPageFor(project);
        }

        private async Task NavigateToProjectPageFor(ProjectProperties project)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ProjectPage)}?{nameof(ProjectProperties)}={project}",
                true,
                new Dictionary<string, object>
                {
                    {nameof(ProjectProperties), project}
                });
        }

        private async void RootFolderChoosingBtn_Clicked(object sender, EventArgs e)
        {
            var pickedFolder = await FolderPicker.PickAsync(default);
            if (pickedFolder is not null && pickedFolder.Folder is not null)
            {
                SettingsManager.SetNewDefaultProjectFolder(pickedFolder.Folder.Path);
                RootFolderLabel.Text = pickedFolder.Folder.Path;
                globalSettings = SettingsManager.RetrieveGlobalSettings();
            }
        }

        private async void CancelNewProjectBtn_Clicked(object sender, EventArgs e)
        {
            await DisplayMainMenuView();
        }

        private async Task DisplayMainMenuView()
        {
            Title = "Brainfuck IDE";
            await NewProjectGrid.ScaleTo(.6, 100);
            NewProjectGrid.IsEnabled = false;
            NewProjectGrid.IsVisible = false;

            MainMenuGrid.IsEnabled = true;
            MainMenuGrid.IsVisible = true;
            await MainMenuGrid.ScaleTo(1d, 100);
        }

        private async void ConfirmNewProjectBtn_Clicked(object sender, EventArgs e)
        {
            if (!IsRootFolderValid(globalSettings.RootFolderForNewProjects))
            {
                ErrorLabel.Text = "Invalid directory chosen. " +
                    "Please, choose existing directory";
                return;
            }
            if (string.IsNullOrEmpty(ErrorLabel.Text))
            {
                MainPageViewModel mainVm = (MainPageViewModel)BindingContext;
                ProjectProperties newProject = mainVm.ProjectToCreate;

                ProjectMetadata newProjMetadata = new()
                {
                    Name = newProject.Name,
                    Path = ProjectStructurator
                        .FormProjectSettingsPath(newProject)
                };
                mainVm.AppendNewProjectToExistingCommand.Execute(newProjMetadata);

                ProjectStructurator.CreateNewProjectStructure(newProject);
                if (shouldCreateWelcomeFileInNewProject)
                {
                    ProjectStructurator.CreateWelcomeFileFor(newProject);
                }
                await DisplayMainMenuView();
                await NavigateToProjectPageFor(newProject);
            }
        }

        private static bool IsRootFolderValid(string? path)
        {
            return path is not null && Directory.Exists(path);
        }

        private void CreateWelcomeFileCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            shouldCreateWelcomeFileInNewProject = e.Value;
        }
    }
}
