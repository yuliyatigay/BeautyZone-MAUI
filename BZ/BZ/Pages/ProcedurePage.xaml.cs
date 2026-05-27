using BZ.ViewModels;
using Domain.Interfaces;
using System.ComponentModel;
using Domain.Models;

namespace BZ.Pages;

public partial class ProcedurePage : ContentPage
{
    private readonly ProcedureViewModel viewModel;
    
    public ProcedurePage(IProcedureService procedureService)
    {
        InitializeComponent();
        viewModel = new ProcedureViewModel(procedureService);
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ProcedureViewModel)BindingContext).LoadProcedures();
    }
    private async void OnProcedureSelected(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is not ProcedureViewModel vm)
            return;

        var procedure = e.CurrentSelection.FirstOrDefault() as Procedure;
        if (procedure == null)
            return;

        await vm.OpenProcedurePopupCommand.ExecuteAsync(procedure);

        ((CollectionView)sender).SelectedItem = null;
    }
}