using Brainf1ck_IDE.ViewModels;

namespace Brainf1ck_IDE;

public partial class ProjectPage : ContentPage
{
	public ProjectPage(ProjectPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}