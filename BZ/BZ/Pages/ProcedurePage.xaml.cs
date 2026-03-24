using BZ.ViewModels;
using Domain.Interfaces;
using System.ComponentModel;
using Domain.Models;

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
        await ((ProcedurePageViewModel)BindingContext).LoadProcedures();
    }
    private async void OnProcedureSelected(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is not ProcedurePageViewModel vm)
            return;

        var procedure = e.CurrentSelection.FirstOrDefault() as Procedure;
        if (procedure == null)
            return;

        await vm.OpenProcedurePopupCommand.ExecuteAsync(procedure);

        ((CollectionView)sender).SelectedItem = null;
    }
}