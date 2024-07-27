using CommunityToolkit.Mvvm.ComponentModel;
using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.Input;

namespace Brainf1ck_IDE.Presentation.ViewModels
{
    [QueryProperty("ProjectProps", nameof(ProjectProperties))]
    [QueryProperty("SelectedFile", nameof(BrainfuckFile))]
    public partial class VisualizerPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProjectProperties projectProps = new();
        [ObservableProperty]
        private BrainfuckFile selectedFile = new();

        [ObservableProperty]
        private string currentCellIndex;
        [ObservableProperty]
        private string currentCellValue = "0";
        [ObservableProperty]
        private char currentInputCharacter = ' ';
        [ObservableProperty]
        string output = string.Empty;
        [ObservableProperty]
        string autorunButtonText = "Turn On Autorun";

        uint stepIntervalInMilliseconds = 700;
        Timer? timer;
        public VisualizerPageViewModel()
        {
            CurrentCellIndex = ProjectProps.InitialCellIndex.ToString();
        }

        [RelayCommand]
        void AutorunButton()
        {
            if (timer is null)
            {
                ToggleAutorun(true);
                AutorunButtonText = "Turn Off Autorun";
            }
            else
            {
                ToggleAutorun(false);
                AutorunButtonText = "Turn On Autorun";
            }
        }

        private void ToggleAutorun(bool shouldEnableAutorun)
        {
            if (shouldEnableAutorun)
            {
                timer = new Timer(TimerCallback, null, 
                    TimeSpan.Zero,
                    TimeSpan.FromMilliseconds(stepIntervalInMilliseconds));
            }
            else
            {
                timer?.Dispose();
            }
        }

        private void TimerCallback(object? state)
        {
            MainThread.BeginInvokeOnMainThread(NextStep);
        }

        private void NextStep()
        {
            // TODO implement
        }

        [RelayCommand]
        void NextStepManually()
        {
            ToggleAutorun(false);
            NextStep();
        }

        [RelayCommand]
        void SlowDownSteps()
        {
            stepIntervalInMilliseconds += 300;
        }
        [RelayCommand]
        void MakeFasterSteps()
        {
            stepIntervalInMilliseconds -= 300;
        }

        [RelayCommand]
        void NotImplemented()
        {
            Output = "This functionality is not yet implemented. Coming soon...";
        }
    }
}
