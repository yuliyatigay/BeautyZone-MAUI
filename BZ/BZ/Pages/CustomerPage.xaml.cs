using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BZ.ViewModels;
using Domain.Interfaces;
using Domain.Models;

namespace BZ.Pages;

public partial class CustomerPage : ContentPage
{
    private readonly CustomerViewModel viewModel;
    public CustomerPage(ICustomerService customerService, IProcedureService procedureService)
    {
        InitializeComponent();
        viewModel = new CustomerViewModel(customerService, procedureService);
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((CustomerViewModel)BindingContext).LoadCustomers();
    }
    private async void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is not CustomerViewModel vm)
            return;

        var customer = e.CurrentSelection.FirstOrDefault() as Customer;
        if (customer == null)
            return;

        await vm.OpenCustomerPopupCommand.ExecuteAsync(customer);

        ((CollectionView)sender).SelectedItem = null;
    }

    private void SearchBar_TextChanged(object? sender, TextChangedEventArgs e)
    {
       viewModel.SearchCustomers(e.NewTextValue);
    }

}