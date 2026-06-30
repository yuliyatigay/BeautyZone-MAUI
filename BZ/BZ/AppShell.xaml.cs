namespace BZ
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Pages.ProcedurePage), typeof(Pages.ProcedurePage));
            Routing.RegisterRoute(nameof(Pages.BeautyTechsPage), typeof(Pages.BeautyTechsPage));
            Routing.RegisterRoute(nameof(Pages.CustomerPage), typeof(Pages.CustomerPage));
            Routing.RegisterRoute(nameof(Pages.Popups.BeautyTechFormPopup), typeof(Pages.Popups.BeautyTechFormPopup));
            Routing.RegisterRoute(nameof(Pages.Popups.CustomerFormPopup), typeof(Pages.Popups.CustomerFormPopup));
            Routing.RegisterRoute(nameof(Pages.Popups.ProcedureFormPopup), typeof(Pages.Popups.ProcedureFormPopup));
        }
    }
}
