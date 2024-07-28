using Brainf1ck_IDE.Common;
using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Brainf1ck_IDE.Presentation.ViewModels
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
        private string output = $"Welcome to {AppInfoHelper.GetAppName()}, " +
            $"version {AppInfoHelper.GetAppVersion()}. Have a nice brainfucking session!";
        [ObservableProperty]
        private Color outputColor = Colors.AliceBlue;

        [ObservableProperty]
        private ObservableCollection<BrainfuckFile> brainfuckFiles = [];
        [ObservableProperty]
        private BrainfuckFile selectedFile = new();

        private string filesFolderPath = string.Empty;
        private string projectSettingsPath = string.Empty;

        private BrainfuckExecutor brainfuckExecutor = new(new());
        private BrainfuckErrorParser brainfuckErrorParser = new(new());

        private ProjectPage projectPage;
        public void ConfigureViewModel(ProjectPage projectPage)
        {
            MemoryLengthInput = ProjectProps.MemoryLength.ToString();
            InitialIndexInput = ProjectProps.InitialCellIndex.ToString();
            filesFolderPath = ProjectValidator
                .FormProjectFilesFolderPath(ProjectProps);
            projectSettingsPath = ProjectValidator
                .FormProjectSettingsPath(ProjectProps);
            BrainfuckFiles = RetrieveBrainfuckFilesData();
            brainfuckExecutor = new(ProjectProps);
            brainfuckErrorParser = new(ProjectProps);
            this.projectPage = projectPage;
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
                    .ReadBrainfuckFile(filePath));
            }
            
            return bfFiles;
        }

        [RelayCommand]
        async Task OpenSettingsView()
        {
            SaveSelectedFile();
            SelectedFile = new();
            await projectPage.OpenSettingsView();
        }

        [RelayCommand]
        async Task TrySaveNewSettings()
        {
            ErrorMessage = ProjectProps.TryPopulateAndGiveFeedback(InitialIndexInput,
                MemoryLengthInput);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ProjectProps.SaveToFile(projectSettingsPath);
                await projectPage.DisplayAlert("Success", "Project settings updated", "Ok");
            }
        }

        [RelayCommand]
        void SelectFile(BrainfuckFile newSelectedFile)
        {
            if (!SelectedFile.Name.Equals(newSelectedFile.Name))
            {
                SaveSelectedFile();
                SelectedFile = newSelectedFile;
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
        async Task DeleteCurrentFile()
        {
            if (SelectedFile is null || string.IsNullOrEmpty(SelectedFile.Name))
            {
                return;
            }

            bool deletionConfirmed = await projectPage.DisplayAlert("Delete selected file",
                $"\"{SelectedFile.Name}\" will be permanently deleted", "Ok", "Cancel");
            if (deletionConfirmed)
            {
                BrainfuckFiles.Remove(SelectedFile);
                string filePath = FormBrainfuckFilePath(SelectedFile);
                Deleter.TryDeleteFile(filePath);
                await projectPage.HideFileView();
            }
        }

        [RelayCommand]
        async Task PasteCodeSnippet()
        {
            string? userAnswer = await projectPage.PromptForSnippet();
            CodeSnippets snippetType = BrainfuckSnippets.GetKeyByString(userAnswer);
            if (snippetType != CodeSnippets.Cancel)
            {
                projectPage.PasteSnippet(snippetType);
            }
        }

        [RelayCommand]
        async Task VisualizeCodeExecution()
        {
            if (brainfuckErrorParser
                    .TryRetrieveErrorsFrom(SelectedFile.Contents, out string errors))
            {
                WriteOutput(errors, BrainfuckOutputTypes.Error);
                return;
            }

            await Shell.Current.GoToAsync(
                $"{nameof(VisualizerPage)}?" +
                $"{nameof(ProjectProperties)}={ProjectProps}" +
                $"&{nameof(BrainfuckFile)}={SelectedFile}",
                true,
                new Dictionary<string, object>
                {
                    { nameof(ProjectProperties), ProjectProps },
                    { nameof(BrainfuckFile), SelectedFile }
                });
        }

        void WriteOutput(string output, BrainfuckOutputTypes type)
        {
            switch (type)
            {
                case BrainfuckOutputTypes.Output:
                    OutputColor = Colors.LawnGreen;
                    break;
                case BrainfuckOutputTypes.Warning:
                    OutputColor = Colors.Yellow;
                    break;
                case BrainfuckOutputTypes.Error:
                    OutputColor = Colors.Red;
                    break;
            }

            Output = output;
        }

        [RelayCommand]
        void ExecuteCode()
        {
            if (brainfuckErrorParser
                    .TryRetrieveErrorsFrom(SelectedFile.Contents, out string errors))
            {
                WriteOutput(errors, BrainfuckOutputTypes.Error);
                return;
            }

            brainfuckExecutor.Execute(SelectedFile.Contents,
                out string executionOutput);
            if (string.IsNullOrWhiteSpace(executionOutput))
            {
                WriteOutput("Your code outputs nothing. " +
                    "Did you forget to write \".\" at the end?",
                    BrainfuckOutputTypes.Warning);
            }
            else
            {
                WriteOutput(executionOutput, BrainfuckOutputTypes.Output);
            }
        }

        [RelayCommand]
        async Task ShowHelp()
        {
            await projectPage.DisplayAlert("Help",
                "Use +- to increment/decrement value of current memory cell.\n" +
                "Use <> to move through memory cells.\n" +
                "Use [] to execute code inside while current cell (last cell before ]) value not equals 0.\n" +
                "Use , to put a character before that symbol (before ,) into current cell.\n" +
                "Use . to output current cell value.\n" +
                "Have fun :)",
                "Ok");
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
