namespace Brainf1ck_IDE
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
        }
    }
}
