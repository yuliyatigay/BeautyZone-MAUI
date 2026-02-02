namespace BZ
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Pages.LoginPage), typeof(Pages.LoginPage));
            Routing.RegisterRoute(nameof(Pages.HomePage), typeof(Pages.HomePage));
            Routing.RegisterRoute(nameof(Pages.ProcedurePage), typeof(Pages.ProcedurePage));
        }
    }
}
