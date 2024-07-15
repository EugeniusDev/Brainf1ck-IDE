using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.ViewModels;

namespace Brainf1ck_IDE
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel mainVm)
        {
            InitializeComponent();
            BindingContext = mainVm;
            // need MVVM for this list, because it's items can be deleted
        }

        private void CreateProjectBtn_Clicked(object sender, EventArgs e)
        {
            // TODO if no default folder exists, prompt user to choose one
            // also user can choose new default folder
            // All of choosings above require updating global settings
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
                    "Valid project file was not selected", "Ok");
                return;
            }

            Project? projectToOpen = StorageReader.RetrieveProjectFrom(chosenFile.FullPath);
            if (projectToOpen is null)
            {
                await DisplayAlert("Selection error",
                    "Valid project file was not selected", "Ok");
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
    }
}
