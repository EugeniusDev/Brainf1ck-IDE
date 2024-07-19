﻿using Brainf1ck_IDE.Common;
using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using Brainf1ck_IDE.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public void ConfigureViewModel()
        {
            MemoryLengthInput = ProjectProps.MemoryLength.ToString();
            InitialIndexInput = ProjectProps.InitialCellIndex.ToString();
            filesFolderPath = ProjectStructurator
                .FormProjectFilesFolderPath(ProjectProps);
            projectSettingsPath = ProjectStructurator
                .FormProjectSettingsPath(ProjectProps);
            BrainfuckFiles = RetrieveBrainfuckFilesData();
            brainfuckExecutor = new(ProjectProps);
            brainfuckErrorParser = new(ProjectProps);
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
        async Task DeleteCurrentFile()
        {
            if (AppShell.Current.CurrentPage is not ProjectPage projectPage)
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
            }
        }

        [RelayCommand]
        void PasteCodeSnippet()
        {

        }

        [RelayCommand]
        void VisualizeCodeExecution()
        {
            if (brainfuckErrorParser
                    .TryRetrieveErrorsFrom(SelectedFile.Contents, out string errors))
            {
                WriteOutput("TODO Find error and display it", BrainfuckOutputTypes.Error);
                return;
            }
            //TODO implement
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
                WriteOutput("TODO Find error and display it", BrainfuckOutputTypes.Error);
                return;
            }

            string executionOutput = brainfuckExecutor
                .Execute(SelectedFile.Contents);
            if (string.IsNullOrEmpty(executionOutput))
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
        void OptimizeCode()
        {
            if (brainfuckErrorParser
                .TryRetrieveErrorsFrom(SelectedFile.Contents, out string errors))
            {
                WriteOutput("TODO Find error and display it", BrainfuckOutputTypes.Error);
                return;
            }
            //TODO implement


        }

        public void SaveAllFiles()
        {
            foreach (var file in BrainfuckFiles)
            {
                SaveBrainfuckFile(file);
            }
        }

        [RelayCommand]
        void NotImplemented()
        {
            if (AppShell.Current.CurrentPage is ProjectPage projectPage)
            {
                projectPage.DisplayAlert("Coming soon...",
                    "This functionality is not yet implemented",
                    "Ok, good luck");
            }
        }
    }
}
