using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Models;
using System.Collections.ObjectModel;

namespace BZ.Pages.Popups;

public partial class ProcedurePickerPopup : Popup
{
    public ObservableCollection<ProcedurePickItem> Items { get; }
    private readonly Action<List<Guid>> _onDone;
    public ProcedurePickerPopup(IEnumerable<Procedure> all,
                            IEnumerable<Guid> selectedIds,
                            Action<List<Guid>> onDone)
    {
        InitializeComponent();

        _onDone = onDone ?? throw new ArgumentNullException(nameof(onDone));

        var selected = new HashSet<Guid>(selectedIds ?? Enumerable.Empty<Guid>());

        Items = new ObservableCollection<ProcedurePickItem>(
            all.Select(p => new ProcedurePickItem
            {
                Id = p.Id,
                Name = p.Name,
                IsSelected = selected.Contains(p.Id)
            }));

        BindingContext = this;
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        var ids = Items.Where(i => i.IsSelected).Select(i => i.Id).ToList();
        _onDone(ids); 
        await CloseAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }


}

public partial class ProcedurePickItem : ObservableObject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    [ObservableProperty]
    private bool isSelected;
}