using CommunityToolkit.Mvvm.ComponentModel;
using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.Input;
using Brainf1ck_IDE.Domain;

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
        ExecutionStepInfo currentStepInfo = new();

        [ObservableProperty]
        string output = string.Empty;
        [ObservableProperty]
        string autorunButtonText = "Turn On Autorun";

        uint stepIntervalInMilliseconds = 700;
        Timer? timer;

        private Queue<ExecutionStepInfo> executionSteps = [];

        public void ConfigureViewModel()
        {
            BrainfuckExecutor executor = new(ProjectProps);
            executionSteps = executor.Execute(SelectedFile.Contents, out _);
        }

        [RelayCommand]
        void AutorunButton()
        {
            if (timer is null)
            {
                ToggleAutorun(true);
            }
            else
            {
                ToggleAutorun(false);
            }
        }

        private void ToggleAutorun(bool shouldEnableAutorun)
        {
            if (shouldEnableAutorun)
            {
                ResetTimer();
                AutorunButtonText = "Turn Off Autorun";
            }
            else
            {
                timer?.Dispose();
                AutorunButtonText = "Turn On Autorun";
            }
        }

        private void ResetTimer()
        {
            timer?.Dispose();
            timer = new Timer(TimerCallback, null,
                TimeSpan.FromMilliseconds(stepIntervalInMilliseconds),
                TimeSpan.FromMilliseconds(stepIntervalInMilliseconds));
        }

        private void TimerCallback(object? state)
        {
            MainThread.BeginInvokeOnMainThread(NextStep);
        }

        private void NextStep()
        {
            if (executionSteps.Count == 0)
            {
                ToggleAutorun(false);
                return;
            }

            CurrentStepInfo = executionSteps.Dequeue();
            Output = string.Concat(Output, CurrentStepInfo.Output);
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
            stepIntervalInMilliseconds += 100;
            ResetTimer();
        }
        [RelayCommand]
        void MakeFasterSteps()
        {
            if (stepIntervalInMilliseconds > 100)
            {
                stepIntervalInMilliseconds -= 100;
                ResetTimer();
            }
        }
    }
}
