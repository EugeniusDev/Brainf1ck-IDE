using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.ViewModels
{
    [QueryProperty("ProjectProps", nameof(ProjectProperties))]
    public partial class ProjectPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProjectProperties projectProps = new();

        [ObservableProperty]
        private string memoryLengthInput = string.Empty;
        [ObservableProperty]
        private string initialIndexInput = string.Empty;
        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private ObservableCollection<BrainfuckFile> brainfuckFiles = [];

        private string filesFolderPath = string.Empty;
        private string projectSettingsPath = string.Empty;

        [ObservableProperty]
        private BrainfuckFile selectedFile = new();
        public void ConfigureViewModel()
        {
            MemoryLengthInput = ProjectProps.MemoryLength.ToString();
            InitialIndexInput = ProjectProps.InitialCellIndex.ToString();
            filesFolderPath = ProjectStructurator
                .FormProjectFilesFolderPath(ProjectProps);
            projectSettingsPath = ProjectStructurator
                .FormProjectSettingsPath(ProjectProps);
            BrainfuckFiles = RetrieveBrainfuckFilesData();

            if (ProjectProps.TargetFileName is not null)
            {
                BrainfuckFile targetFile = BrainfuckFiles
                    .First(file =>
                        file.Name.Equals(ProjectProps.TargetFileName));
                SelectFile(targetFile);
            }
        }

        private ObservableCollection<BrainfuckFile> RetrieveBrainfuckFilesData()
        {
            var bfFilepaths = Directory.GetFiles(filesFolderPath)
                .Where(filePath => 
                    Path.GetExtension(filePath)
                        .Equals(".bf", StringComparison.OrdinalIgnoreCase)
                );
            ObservableCollection<BrainfuckFile> bfFiles = [];
            foreach (var filePath in bfFilepaths)
            {
                bfFiles.Add(StorageReader
                    .RetrieveBrainfuckFile(filePath));
            }
            
            return bfFiles;
        }

        [RelayCommand]
        void OpenSettingsView()
        {
            SaveSelectedFile();
            SelectedFile = new();
        }

        [RelayCommand]
        async Task TrySaveNewSettings()
        {
            ErrorMessage = ProjectProps.ParseInputWithFeedback(InitialIndexInput,
                MemoryLengthInput);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ProjectProps.SaveToFile(projectSettingsPath);
                if (AppShell.Current.CurrentPage is not ProjectPage projectPage)
                {
                    return;
                }

                await projectPage.DisplayAlert("Success", "Project settings updated", "Ok");
            }
        }

        [RelayCommand]
        void SelectFile(BrainfuckFile newSelectedFile)
        {
            if (SelectedFile is not null)
            {
                SaveSelectedFile();
            }

            SelectedFile = newSelectedFile;
        }

        [RelayCommand]
        void SelectFileTest(string newSelectedFile)
        {
            if (AppShell.Current.CurrentPage is ProjectPage projectPage)
            {
                projectPage.DisplayAlert("yeah", "it\'s string issue", "Ok");
            }
        }

        private void SaveSelectedFile()
        {
            SaveBrainfuckFile(SelectedFile);
        }

        private void SaveBrainfuckFile(BrainfuckFile file)
        {
            if (file is not null
                && !string.IsNullOrWhiteSpace(file.Name))
            {
                StorageWriter.SaveFile(FormBrainfuckFilePath(file), file.Contents);
            }
        }

        private string FormBrainfuckFilePath(BrainfuckFile chosenFile)
        {
            return Path.Combine(filesFolderPath, chosenFile.Name);
        }

        [RelayCommand]
        async Task AddNewFile()
        {
            if (AppShell.Current.CurrentPage is not ProjectPage projectPage)
            {
                return;
            }

            string newFileName = await projectPage.PromptNewBrainfuckFilename();
            if (string.IsNullOrEmpty(newFileName))
            {
                return;
            }
            if (FilenameAlreadyInUse(newFileName))
            {
                await projectPage.DisplayAlert("Choose another name",
                    "This file already exists in a project", "Ok");
                return;
            }

            BrainfuckFile newFile = new()
            {
                Name = newFileName
            };
            BrainfuckFiles.Add(newFile);
            SelectFile(newFile);
        }

        private bool FilenameAlreadyInUse(string fileName)
        {
            List<string> names = [];
            foreach (var file in BrainfuckFiles)
            {
                names.Add(file.Name);
            }
            return names.Contains(fileName);
        }

        [RelayCommand]
        async Task DeleteFile(BrainfuckFile chosenFile)
        {
            if (AppShell.Current.CurrentPage is not ProjectPage projectPage)
            {
                return;
            }
            bool deletionConfirmed = await projectPage.DisplayAlert("Delete selected file",
                "Selected file will be permanently deleted", "Ok", "Cancel");
            if (deletionConfirmed)
            {
                BrainfuckFiles.Remove(chosenFile);
                string filePath = FormBrainfuckFilePath(chosenFile);
                Deleter.TryDeleteFile(filePath);
            }
        }

        [RelayCommand]
        void PasteCodeSnippet()
        {

        }

        public void SaveAllFiles()
        {
            foreach (var file in BrainfuckFiles)
            {
                SaveBrainfuckFile(file);
            }
        }
    }
}
