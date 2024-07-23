using System.Windows.Input;

namespace Brainf1ck_IDE;

public partial class TextImageFrame : ContentView
{
    public static readonly BindableProperty CommandToCallProperty =
            BindableProperty.Create(nameof(CommandToCall), typeof(ICommand), typeof(TextImageFrame));

    public static readonly BindableProperty ImageSrcProperty =
        BindableProperty.Create(nameof(ImageSrc), typeof(ImageSource), typeof(TextImageFrame));

    public static readonly BindableProperty PrimaryTextProperty =
        BindableProperty.Create(nameof(PrimaryText), typeof(string), typeof(TextImageFrame));
    public static readonly BindableProperty PrimaryFontSizeProperty =
        BindableProperty.Create(nameof(PrimaryTextFontSize), typeof(string), typeof(TextImageFrame));

    public ICommand CommandToCall
    {
        get => (ICommand)GetValue(CommandToCallProperty);
        set => SetValue(CommandToCallProperty, value);
    }

    public ImageSource ImageSrc
    {
        get => (ImageSource)GetValue(ImageSrcProperty);
        set => SetValue(ImageSrcProperty, value);
    }

    public string PrimaryText
    {
        get => (string)GetValue(PrimaryTextProperty);
        set => SetValue(PrimaryTextProperty, value);
    }
    public string PrimaryTextFontSize
    {
        get => (string)GetValue(PrimaryFontSizeProperty);
        set => SetValue(PrimaryFontSizeProperty, value);
    }

    public TextImageFrame()
	{
		InitializeComponent();
	}
}