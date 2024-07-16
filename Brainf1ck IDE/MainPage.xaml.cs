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
        public MainPage(MainPageModel mainVm)
        {
            InitializeComponent();
            BindingContext = mainVm;
        }

        private async void CreateProjectBtn_Clicked(object sender, EventArgs e)
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

        private async void OpenProjectBtn_Clicked(object sender, EventArgs e)
        {
            var chosenFile = await FilePicker.PickAsync(
                new PickOptions{
                    PickerTitle = $"Please select file with " +
                    $"\"{FilePaths.ideRelatedFileExtension}\" extension",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        {DevicePlatform.Android, new[] { $"{FilePaths.ideRelatedFileExtension}" } },// TODO check if any issues appear because of ideRelatedFileExtension returns extension with dot
                        {DevicePlatform.WinUI, new[] { $"{FilePaths.ideRelatedFileExtension}" } }
                    })
                });
            if (chosenFile is null)
            {
                await DisplayAlert("Selection error", 
                    "Project file was not selected", "Ok");
                return;
            }

            ProjectProperties? projectToOpen = StorageReader.RetrieveProjectFrom(chosenFile.FullPath);
            if (projectToOpen is null)
            {
                await DisplayAlert("Selection error",
                    "Project file data is corrupt", "Ok");
                return;
            }

            MainPageModel viewModel = (MainPageModel)BindingContext;
            ProjectMetadata newExistingProject = new()
            {
                Name = projectToOpen.Name,
                Path = chosenFile.FullPath
            };
            viewModel.AppendNewProjectToExisting(newExistingProject);
        }

        private void RootFolderChoosingBtn_Clicked(object sender, EventArgs e)
        {
            // TODO implement directorypick
            
            if (true)
            {
                SettingsManager.SetNewDefaultFolder("testpath");
                globalSettings = SettingsManager.RetrieveGlobalSettings();
            }
        }

        private async void CancelNewProjectBtn_Clicked(object sender, EventArgs e)
        {
            Title = "Brainfuck IDE";
            await NewProjectGrid.ScaleTo(.6, 100);
            NewProjectGrid.IsEnabled = false;
            NewProjectGrid.IsVisible = false;

            MainMenuGrid.IsEnabled = true;
            MainMenuGrid.IsVisible = true;
            await MainMenuGrid.ScaleTo(1d, 100);
        }

        private void ConfirmNewProjectBtn_Clicked(object sender, EventArgs e)
        {
            if (!IsRootFolderValid(globalSettings.RootFolderForNewProjects))
            {
                ErrorLabel.Text = "Invalid directory chosen. " +
                    "Please, choose existing directory";
                return;
            }
            if (string.IsNullOrEmpty(ErrorLabel.Text))
            {
                // TODO make extension method for
                // creating and opening it, if possible
                DisplayAlert("Test", "Working", "Ok");
                MainPageModel mainVm = (MainPageModel)BindingContext;
                mainVm.ProjectToCreate.SaveToFile("test");
            }
        }

        private static bool IsRootFolderValid(string? path)
        {
            return path is not null && Directory.Exists(path);
        }

        private void DefaultFileCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            MainPageModel mainVm = (MainPageModel)BindingContext;
            bool isCheckboxChecked = e.Value;
            if (isCheckboxChecked)
            {
                mainVm.ProjectToCreate.SetBackDefaultFileToRun();
            }
            else
            {
                mainVm.ProjectToCreate.FileToRun = null;
            }
        }
    }
}
