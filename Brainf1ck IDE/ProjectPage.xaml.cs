using Brainf1ck_IDE.Common;
using Brainf1ck_IDE.Common.FileProcessing;
using Brainf1ck_IDE.Domain;
using Brainf1ck_IDE.Presentation.ViewModels;

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
		    vm.ConfigureViewModel(this);
        }

        base.OnAppearing();
    }

    public async Task OpenSettingsView()
    {
        await ToggleNoActiveViewLabel(false);
        await DisplaySettingsView();
    }

    private async Task ToggleNoActiveViewLabel(bool turnOn)
    {
        if (turnOn)
        {
            NoActiveViewLabel.IsVisible = true;
            NoActiveViewLabel.IsEnabled = true;
            await NoActiveViewLabel.ScaleTo(1d, 100);
        }
        else if (NoActiveViewLabel.IsVisible)
        {
            await NoActiveViewLabel.ScaleTo(.6, 100);
            NoActiveViewLabel.IsVisible = false;
            NoActiveViewLabel.IsEnabled = false;
        }
    }

    private async Task DisplaySettingsView()
    {
        await HideCodeView();

        SettingsView.IsEnabled = true;
        SettingsView.IsVisible = true;
        await SettingsView.ScaleTo(1d, 100);
    }

    private async Task HideCodeView()
    {
        await CodeGrid.ScaleTo(.6, 100);
        CodeGrid.IsEnabled = false;
        CodeGrid.IsVisible = false;
    }

    public async Task HideFileView()
    {
        await HideCodeView();
        await ToggleNoActiveViewLabel(true);
    }

    private async void BrainfuckFile_Tapped(object sender, TappedEventArgs e)
    {
        await ToggleNoActiveViewLabel(false);
        await DisplayCodeView();
    }

    private async Task DisplayCodeView()
    {
        await SettingsView.ScaleTo(.6, 100);
        SettingsView.IsEnabled = false;
        SettingsView.IsVisible = false;

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

        if (!FileValidator.IsStringValidForFilesystem(input))
        {
            await DisplayAlert("Invalid file name",
                "It can contain only English literals, digits and underscore",
                "Ok");
            return string.Empty;
        }

        return string.Concat(input, ".bf");
    }

    public async Task<string?> PromptForSnippet()
    {
        return await DisplayActionSheet("Choose snippet to paste",
            "Cancel",
            "Hello World");
    }

    public void PasteSnippet(CodeSnippets type)
    {
        string currentText = TextEditor.Text;
        int cursorPosition = TextEditor.CursorPosition;

        string newText = string.Concat(currentText[..cursorPosition],
            BrainfuckSnippets.GetMarkedSnippetByKey(type),
            currentText[cursorPosition..]);
        TextEditor.Text = newText;
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