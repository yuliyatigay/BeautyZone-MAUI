using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;

namespace BZ.ViewModels;

public partial class ProcedurePageViewModel : ObservableObject
{
    private readonly IProcedureService _procedureService;

    [ObservableProperty]
    private ObservableCollection<Procedure> procedures = new();

    [ObservableProperty]
    private Procedure selectedProcedure;

    public ProcedurePageViewModel(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }

    public async Task<ObservableCollection<Procedure>> LoadProcedures()
    {
        Procedures = new ObservableCollection<Procedure>
            (await _procedureService.GetAllProcedures());
        return Procedures;
    }

    [RelayCommand]
    public async Task Delete()
    {

    }
}
