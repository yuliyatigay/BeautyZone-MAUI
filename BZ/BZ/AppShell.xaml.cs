namespace BZ
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Pages.ProcedurePage), typeof(Pages.ProcedurePage));
            Routing.RegisterRoute(nameof(Pages.ProcedureFormPage), typeof(Pages.ProcedureFormPage));
            Routing.RegisterRoute(nameof(Pages.BeautyTechsPage), typeof(Pages.BeautyTechsPage));
            Routing.RegisterRoute(nameof(Pages.BeautyTechFormPage), typeof(Pages.BeautyTechFormPage));
        }
    }
}
