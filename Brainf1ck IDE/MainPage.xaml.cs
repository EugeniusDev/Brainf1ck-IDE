using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Common.Settings;
using Brainf1ck_IDE.Presentation.ViewModels;
using CommunityToolkit.Maui.Storage;

namespace Brainf1ck_IDE
{
    public partial class MainPage : ContentPage
    {
        private GlobalSettings globalSettings = SettingsManager.RetrieveGlobalSettings();
        private bool shouldCreateWelcomeFileInNewProject = true;
        private readonly MainPageViewModel viewModel;
        public MainPage(MainPageViewModel mainVm)
        {
            InitializeComponent();
            viewModel = mainVm;
            BindingContext = viewModel;
            viewModel.BindToMainPage(this);
        }

        private async void CreateProjectViewBtn_Tapped(object sender, EventArgs e)
        {
            await DisplayCreateNewProjectView();

            ProjectNameEntry.Focus();
            if (globalSettings.RootFolderForNewProjects is not null)
            {
                RootFolderLabel.Text = globalSettings.RootFolderForNewProjects;
            }
        }

        private async Task DisplayCreateNewProjectView()
        {
            Title = "Create new Brainfuck project";
            await MainMenuGrid.ScaleTo(.6, 100);
            MainMenuGrid.IsEnabled = false;
            MainMenuGrid.IsVisible = false;

            NewProjectGrid.IsEnabled = true;
            NewProjectGrid.IsVisible = true;
            await NewProjectGrid.ScaleTo(1d, 100);
        }

        private async void OpenProjectBtn_Tapped(object sender, TappedEventArgs e)
        {
            FileResult? chosenFile = await FilePaths.PickFilePath();
            if (chosenFile is null)
            {
                return;
            }

            ProjectProperties? projectToOpen = StorageReader
                .ReadProjectPropertiesFrom(chosenFile.FullPath);
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
            viewModel.AppendNewProjectToExisting(newExistingProject);

            await TryOpenProject(projectToOpen, newExistingProject.Path);
        }

        public async Task TryOpenProject(ProjectProperties? project, string projectPropsFilePath)
        {
            if (project is null)
            {
                await DisplayAlert("Project data corrupt", "Can not open project", "Ok");
                return;
            }

            SettingsManager
                .SetNewDefaultProjectFolderFromSettingsFile(projectPropsFilePath);
            ProjectValidator.EnsureCorrectStructure(project);

            await project.OpenInProjectPage();
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

        public async Task TryFinalizeProjectCreation(ProjectProperties project)
        {
            if (!FileValidator.IsRootFolderValid(globalSettings
                .RootFolderForNewProjects))
            {
                ErrorLabel.Text = "Invalid directory chosen. " +
                    "Please, choose existing directory";
                return;
            }

            if (string.IsNullOrEmpty(ErrorLabel.Text))
            {
                ProjectMetadata newProjMetadata = new()
                {
                    Name = project.Name,
                    Path = ProjectValidator
                        .FormProjectSettingsPath(project)
                };
                viewModel.AppendNewProjectToExistingCommand.Execute(newProjMetadata);

                ProjectValidator.EnsureCorrectStructure(project);
                if (shouldCreateWelcomeFileInNewProject)
                {
                    ProjectValidator.CreateWelcomeFileFor(project);
                }
                await DisplayMainMenuView();
                await project.OpenInProjectPage();
            }
        }

        private void CreateWelcomeFileCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            shouldCreateWelcomeFileInNewProject = e.Value;
        }
    }
}
