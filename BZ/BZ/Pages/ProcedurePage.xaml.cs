using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class ProcedurePage : ContentPage
{
    private readonly ProcedurePageViewModel viewModel;
    public ProcedurePage(IProcedureService procedureService)
    {
        InitializeComponent();
        viewModel = new ProcedurePageViewModel(procedureService);
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadProcedures();
    }
}