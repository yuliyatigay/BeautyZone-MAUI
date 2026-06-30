using System.Collections.ObjectModel;
using BZ.Pages.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;

namespace BZ.ViewModels;

public partial class CustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly IProcedureService _procedureService;
    [ObservableProperty]
    private ObservableCollection<Customer> customers= new();
    private ObservableCollection<Customer> allCustomers;

    [ObservableProperty] 
    private Customer newCustomer = new();
    [ObservableProperty]
    private bool isRefreshing;
    [ObservableProperty]
    private bool isLoading;

    public CustomerViewModel(ICustomerService customerService, IProcedureService procedureService)
    {
        _customerService = customerService;
        _procedureService = procedureService;
    }
    public async Task LoadCustomers()
    {
        try
        {
            IsLoading = true;
            var list = await _customerService.GetAllCustomers();
            allCustomers = new ObservableCollection<Customer>(list);
            Customers = new ObservableCollection<Customer>(list);
            
        }
        finally
        {
            IsLoading = false;
        }
    }
    [RelayCommand]
    public async Task Delete(Customer customer)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление",
            "Вы действительно хотите удалить этого клиента?",
            "Да",
            "Нет");

        if (!confirm)
            return;

        await _customerService.Delete(customer.Id);

        var deleted = Customers.FirstOrDefault(p => p.Id == customer.Id);

        if (deleted != null)
        {
            Customers.Remove(deleted);
        }

        await Shell.Current.DisplayAlert("Готово", " Клиент удален успешно", "OK");
    }
    

    [RelayCommand]
    public async Task OpenCustomerPopup(Customer customer)
    {
        var popup = new CustomerFormPopup(customer ,async edited=>Submit(edited));
            
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }

    [RelayCommand]
    public async Task Refresh()
    {
        try
        {
            IsRefreshing = true;
            var list = await _customerService.GetAllCustomers(true);
            Customers = new ObservableCollection<Customer>(list);
            IsRefreshing = false;
        }
        finally
        {
            isRefreshing = false;
        }
    }
    public async Task Submit(Customer customer)
    {
        
        if (customer.Id == Guid.Empty)
        {
            var result = await _customerService.CreateCustomer(customer);
            if(result != null)
            {
                await Shell.Current.DisplayAlert("Готово", " Клиент создан успешно", "OK");
            }

            await Refresh();
        }
        else
        {
            var result = await _customerService.UpdateCustomer(customer);
            if (result)
            {
                await Shell.Current.DisplayAlert("Готово", "Клиент обновлен успешно", "OK");
            }
            await Refresh();
        }
    }

    public void SearchCustomers(string filterText)
    {
        if (string.IsNullOrWhiteSpace(filterText))
        {
            Customers = new ObservableCollection<Customer>(allCustomers);
            return;
        }

        var searched = allCustomers
            .Where(x =>
                (!string.IsNullOrWhiteSpace(x.Name) &&
                 x.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                ||
                (!string.IsNullOrWhiteSpace(x.PhoneNumber) &&
                 x.PhoneNumber.Contains(filterText))).ToList();
        
        Customers = new ObservableCollection<Customer>(searched);
    }
}
public partial class PhonePickerItem : ObservableObject
{
    [ObservableProperty]
    private string? selectedPhone;
}