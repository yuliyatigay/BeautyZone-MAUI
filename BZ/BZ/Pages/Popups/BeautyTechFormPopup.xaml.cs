using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Domain.Models;

namespace BZ.Pages.Popups;

public partial class BeautyTechFormPopup : Popup
{
    public BeautyTech Specialist {get; }
    public ObservableCollection<Procedure> AvailableProcedures { get; }
    public ObservableCollection<ProcedurePickerItem> ProcedurePickers { get; }
    public Func<BeautyTech, Task> _onDone;
    public string PopupTitle { get; set; }
    public BeautyTechFormPopup(BeautyTech specialist, IEnumerable<Procedure> availableProcedures, 
                                Func<BeautyTech, Task> onDone)
    {
        InitializeComponent();
        _onDone = onDone;
        PopupTitle = "Редактировать мастера";
        Specialist = new BeautyTech
        {
            Id = specialist.Id,
            Name = specialist.Name,
            PhoneNumber =  specialist.PhoneNumber,
            Procedures = specialist.Procedures
        };
        
        AvailableProcedures = new ObservableCollection<Procedure>(availableProcedures);

        ProcedurePickers = new ObservableCollection<ProcedurePickerItem>(
            (Specialist.Procedures ?? Enumerable.Empty<Procedure>())
            .Select(p => new ProcedurePickerItem
            {
                SelectedProcedure = p
            }));
        if (ProcedurePickers.Count == 0)
        {
            PopupTitle = "Создать мастера";
            ProcedurePickers.Add(new ProcedurePickerItem());
        }
        
        BindingContext = this;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        Specialist.Procedures = ProcedurePickers
            .Where(x => x.SelectedProcedure is not null)
            .Select(x => x.SelectedProcedure!)
            .GroupBy(x => x.Id)
            .Select(g => g.First())
            .ToList();

        if (_onDone != null)
            await _onDone(Specialist);

        await CloseAsync();
    }
    private void OnAddPickerClicked(object sender, EventArgs e)
    {
        ProcedurePickers.Add(new ProcedurePickerItem());
    }

    private void OnRemovePickerClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ProcedurePickerItem item)
        {
            if (ProcedurePickers.Count > 1)
                ProcedurePickers.Remove(item);
        }
    }

}