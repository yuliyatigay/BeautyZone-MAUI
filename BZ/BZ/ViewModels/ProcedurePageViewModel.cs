using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;

namespace BZ.ViewModels;

public partial class ProcedurePageViewModel : ObservableObject
{
    private readonly IProcedureService _procedureService;
    public Guid ProcedureId { get; set; }

    [ObservableProperty]
    private ObservableCollection<Procedure> procedures = new();
    [ObservableProperty]
    private bool isRefreshing;

    public ProcedurePageViewModel(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }

    [ObservableProperty]
    private bool isLoading;

    public async Task LoadProcedures()
    {
        try
        {
            IsLoading = true;
            var list = await _procedureService.GetAllProcedures();
            Procedures = new ObservableCollection<Procedure>(list);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task Refresh()
    {
        try
        {
            IsRefreshing = true;
            var list = await _procedureService.GetAllProcedures(true);
            Procedures = new ObservableCollection<Procedure>(list);
            IsRefreshing = false;
        }
        finally
        {
            isRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task GoToCreateProcedure()
    {
        await Shell.Current.GoToAsync(nameof(CreateProcedurePage));
    }

    [RelayCommand]
    public async Task Delete(Guid id)
    {
        await _procedureService.Delete(id);
        var deleted = Procedures.FirstOrDefault(p => p.Id == id);

        if (deleted != null)
        {
            Procedures.Remove(deleted);
            await Shell.Current.DisplayAlert("Готово", "Процедура удалена успешно", "OK");
        }
            
    }

    [RelayCommand]
    public async Task GoToEditProcedure(Procedure procedure)
    {
        Dictionary<string, object> query = new()
        {
                { nameof(Procedure), procedure}
        };
        await Shell.Current.GoToAsync(nameof(CreateProcedurePage), query);
    }
}
