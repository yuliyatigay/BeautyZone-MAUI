using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;
using BZ.Pages.Popups;
using CommunityToolkit.Maui.Extensions;

namespace BZ.ViewModels;

public partial class ProcedurePageViewModel : ObservableObject
{
    private readonly IProcedureService _procedureService;

    [ObservableProperty]
    private ObservableCollection<Procedure> procedures = new();
    [ObservableProperty]
    private bool isRefreshing;
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty] 
    private Procedure newProcedure = new();


    public ProcedurePageViewModel(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }

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
    public async Task Delete(Procedure procedure)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление",
            "Вы действительно хотите удалить эту процедуру?",
            "Да",
            "Нет");

        if (!confirm)
            return;

        await _procedureService.Delete(procedure.Id);

        var deleted = Procedures.FirstOrDefault(p => p.Id == procedure.Id);

        if (deleted != null)
        {
            Procedures.Remove(deleted);
        }

        await Shell.Current.DisplayAlert("Готово", "Процедура удалена успешно", "OK");
    }
    

    [RelayCommand]
    public async Task OpenProcedurePopup(Procedure procedure)
    {

        var popup = new ProcedureFormPopup(procedure.Name, procedure.Id, async edited=>Submit(edited));
            
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    
    public async Task Submit(Procedure procedure)
    {
        
        if (procedure.Id == Guid.Empty)
        {
            var result = await _procedureService.CreateProcedure(procedure.Name);
            if(result != null)
            {
                await Shell.Current.DisplayAlert("Готово", "Процедура создана успешно", "OK");
            }
            await Shell.Current.GoToAsync(nameof(ProcedurePage));
        }
        else
        {
            var result = await _procedureService.UpdateProcedure(procedure);
            if (result)
            {
                await Shell.Current.DisplayAlert("Готово", "Процедура обновлена успешно", "OK");
            }
            await Shell.Current.GoToAsync(nameof(ProcedurePage));
        }
    }
}
