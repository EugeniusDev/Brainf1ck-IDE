using System.Windows.Input;

namespace Brainf1ck_IDE;

public partial class DblTextImageGrid : ContentView
{
    public static readonly BindableProperty ImageSrcProperty =
        BindableProperty.Create(nameof(ImageSrc), typeof(ImageSource), typeof(TextImageFrame));

    public static readonly BindableProperty PrimaryTextProperty =
        BindableProperty.Create(nameof(PrimaryText), typeof(string), typeof(TextImageFrame));

    public static readonly BindableProperty SecondaryTextProperty =
        BindableProperty.Create(nameof(SecondaryText), typeof(string), typeof(TextImageFrame));

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

    public string SecondaryText
    {
        get => (string)GetValue(SecondaryTextProperty);
        set => SetValue(SecondaryTextProperty, value);
    }

    public DblTextImageGrid()
	{
		InitializeComponent();
	}
}