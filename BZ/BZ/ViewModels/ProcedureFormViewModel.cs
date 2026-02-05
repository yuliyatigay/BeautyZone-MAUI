using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;

namespace BZ.ViewModels;

public partial class ProcedureFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly IProcedureService _procedureService;
    
    public ProcedureFormViewModel(IProcedureService procedureService)
    {
        _procedureService = procedureService;
        
    }
    [ObservableProperty]
    private string procedureName = string.Empty;

    public Procedure procedure { get; set; }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.GoToAsync("..", true);
    }
    [RelayCommand]
    public async Task Submit()
    {
        if (procedure is null)
        {
            var result = await _procedureService.CreateProcedure(ProcedureName);
            if(result != null)
            {
                await Shell.Current.DisplayAlert("Готово", "Процедура создана успешно", "OK");
            }
            await Shell.Current.GoToAsync(nameof(ProcedurePage));
        }
        else
        {
            procedure.Name = ProcedureName;
            var result = await _procedureService.UpdateProcedure(procedure);
            if (result)
            {
                await Shell.Current.DisplayAlert("Готово", "Процедура обновлена успешно", "OK");
            }
            await Shell.Current.GoToAsync(nameof(ProcedurePage));
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query != null &&
       query.TryGetValue(nameof(Procedure), out var value) &&
       value is Procedure p)
        {
            procedure = p;
            ProcedureName = p.Name;
        }
        else
        {
            procedure = null;
        }
    }
}
