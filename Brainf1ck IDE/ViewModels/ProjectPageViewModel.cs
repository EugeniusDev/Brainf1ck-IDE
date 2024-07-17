using Brainf1ck_IDE.Common.ProjectRelated;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Brainf1ck_IDE.ViewModels
{
    [QueryProperty("ProjectProps", nameof(ProjectProperties))]
    public partial class ProjectPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProjectProperties projectProps = new();

        [ObservableProperty]
        private string memoryLengthInput;
        [ObservableProperty]
        private string initialIndexInput;
        [ObservableProperty]
        private string errorMessage = string.Empty;
        public ProjectPageViewModel()
        {
            memoryLengthInput = projectProps.MemoryLength.ToString();
            initialIndexInput = projectProps.InitialCellIndex.ToString();
        }
    }
}
