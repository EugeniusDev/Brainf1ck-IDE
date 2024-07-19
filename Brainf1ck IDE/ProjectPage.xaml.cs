using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.ViewModels;

namespace Brainf1ck_IDE;

public partial class ProjectPage : ContentPage
{
	public ProjectPage(ProjectPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override void OnAppearing()
    {
        if (BindingContext is ProjectPageViewModel vm)
        {
		    vm.ConfigureViewModel();
        }

        base.OnAppearing();
    }

    private async void SettingsFrame_Tapped(object sender, TappedEventArgs e)
    {
        await DisplaySettingsView();
    }

    private async Task DisplaySettingsView()
    {
        if (NoActiveViewLabel.IsVisible)
        {
            NoActiveViewLabel.IsVisible = false;
            NoActiveViewLabel.IsEnabled = false;
        }
        else
        {
            await CodeGrid.ScaleTo(.6, 100);
            CodeGrid.IsEnabled = false;
            CodeGrid.IsVisible = false;
        }

        SettingsGrid.IsEnabled = true;
        SettingsGrid.IsVisible = true;
        await SettingsGrid.ScaleTo(1d, 100);
    }

    private async void FilesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await DisplayCodeView();
    }

    private async Task DisplayCodeView()
    {
        if (NoActiveViewLabel.IsVisible)
        {
            NoActiveViewLabel.IsVisible = false;
            NoActiveViewLabel.IsEnabled = false;
        }
        else
        {
            await SettingsGrid.ScaleTo(.6, 100);
            SettingsGrid.IsEnabled = false;
            SettingsGrid.IsVisible = false;
        }

        CodeGrid.IsEnabled = true;
        CodeGrid.IsVisible = true;
        await CodeGrid.ScaleTo(1d, 100);
    }

    public async Task<string> PromptNewBrainfuckFilename()
    {
        string input = await DisplayPromptAsync("Enter file name",
            "It can contain only English literals, digits and underscore");
        if (input is null)
        {
            return string.Empty;
        }

        if (FileValidator.IsStringValidForFilesystem(input))
        {
            await DisplayAlert("Invalid file name",
                "It can contain only English literals, digits and underscore",
                "Ok");
            return string.Empty;
        }

        return string.Concat(input, ".bf");
    }

    protected override void OnDisappearing()
    {
        if (BindingContext is ProjectPageViewModel vm)
        {
            vm.SaveAllFiles();
        }

        base.OnDisappearing();
    }
}