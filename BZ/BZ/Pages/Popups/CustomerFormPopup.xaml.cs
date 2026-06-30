using System.Collections.ObjectModel;
using BZ.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Models;

namespace BZ.Pages.Popups;

public partial class CustomerFormPopup : Popup
{
    public Customer Customer { get;  }
    private readonly Func<Customer, Task> _onDone;
    public string PopupTitle { get; set; }
    public CustomerFormPopup(Customer customer, Func<Customer, Task> onDone)
    {
        InitializeComponent();
        if (customer.Id == Guid.Empty)
        {
            PopupTitle = "Создать клиента";
            Customer = new Customer();
        }
        else
        { 
            PopupTitle = "Редактировать клиента";
            Customer = new Customer
            {
                Name = customer.Name,
                Id = customer.Id,
                PhoneNumber = customer.PhoneNumber
            };
        }
        _onDone = onDone;
        BindingContext = this;
        
    }
    
    private async void OnDoneClicked(object sender, EventArgs e)
    {
        if (_onDone != null)
            await _onDone(Customer);
        await CloseAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}
