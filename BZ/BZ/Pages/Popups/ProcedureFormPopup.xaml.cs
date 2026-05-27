using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Domain.Models;
using UXDivers.Popups.Maui;

namespace BZ.Pages.Popups;

public partial class ProcedureFormPopup : Popup
{
    public Procedure Procedure { get;  }
    private readonly Func<Procedure, Task> _onDone;
    public string PopupTitle { get; set; }
    public ProcedureFormPopup(string procedureName, Guid selectedId, Func<Procedure, Task> onDone)
    {
        InitializeComponent();
        if (string.IsNullOrWhiteSpace(procedureName))
        {
            PopupTitle = "Создать процедуру";
        }
        else
        { 
            PopupTitle = "Редактировать процедуру";
        }
        Procedure = new Procedure
        {
            Name = procedureName,
            Id = selectedId
        };
        _onDone = onDone;
        BindingContext = this;
        
    }
    
    private async void OnDoneClicked(object sender, EventArgs e)
    {
        if (_onDone != null)
            await _onDone(Procedure);
        await CloseAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}