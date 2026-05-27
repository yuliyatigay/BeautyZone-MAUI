using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Domain.Models;

namespace BZ.Pages.Popups;

public partial class CustomerFormPopup : Popup
{
    public Customer Customer { get; }
    private Func<Customer, Task> _onDone;
    public string PopupTitle { get; set; }
    public CustomerFormPopup(Customer customer, Func<Customer, Task> onDone)
    {
        InitializeComponent();
        PopupTitle = "Редактировать клиента";

        if (Customer == null)
        {
            PopupTitle = "Создать клиента";
        }
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDoneClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}