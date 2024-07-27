using Brainf1ck_IDE.Presentation.ViewModels;

namespace Brainf1ck_IDE;

public partial class VisualizerPage : ContentPage
{
	public VisualizerPage(VisualizerPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}