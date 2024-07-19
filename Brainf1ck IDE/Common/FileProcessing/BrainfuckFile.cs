using CommunityToolkit.Mvvm.ComponentModel;

namespace Brainf1ck_IDE.Common.FileProcessing
{
    public partial class BrainfuckFile : ObservableObject
    {
        [ObservableProperty]
        string name = string.Empty;
        [ObservableProperty]
        string contents = string.Empty;
    }
}
