using BZ.Pages;
using BZ.Pages.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;

namespace BZ.ViewModels;

public partial class BeautyTechFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly IBeautyTechService _beautyTechService;
    private readonly IProcedureService _procedureService;
    public BeautyTechFormViewModel(IBeautyTechService beautyTechService, IProcedureService procedureService)
    {
        _beautyTechService = beautyTechService;
        _procedureService = procedureService;
    }
    [ObservableProperty]
    private ObservableCollection<Procedure> procedures = new();
    [ObservableProperty]
    private ObservableCollection<Guid> selectedProcedureIds = new();
    
    [ObservableProperty] private string name = "";
    [ObservableProperty] private string phoneNumber = "";

    public List<Procedure> SelectedProcedures =>
    Procedures.Where(p => SelectedProcedureIds.Contains(p.Id)).ToList();

    public string SelectedProceduresText =>
    SelectedProcedures.Count == 0
        ? "Процедуры не выбраны"
        : string.Join(", ", SelectedProcedures.Select(p => p.Name));
    public BeautyTech? beautyTech { get; set; }

    public async Task LoadProcedures()
    {
        if (Procedures.Count > 0) return; // чтобы не грузить повторно
        var list = await _procedureService.GetAllProcedures();
        Procedures = new ObservableCollection<Procedure>(list);
        OnPropertyChanged(nameof(SelectedProceduresText));
        OnPropertyChanged(nameof(SelectedProcedures));
    }

    [RelayCommand]
    public async Task OpenProceduresPopup()
    {
        await LoadProcedures();

        var popup = new ProcedurePickerPopup(
            Procedures,
            SelectedProcedureIds,
            ids =>
            {
                SelectedProcedureIds = new ObservableCollection<Guid>(ids);
                OnPropertyChanged(nameof(SelectedProceduresText));
                OnPropertyChanged(nameof(SelectedProcedures));
            });

        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }


    [RelayCommand]
    public async Task Submit()
    {
        // собираем объект для отправки
        if (beautyTech is null)
        {
            var created = new BeautyTech
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                Procedures = SelectedProcedureIds.ToList()
            };

            var result = await _beautyTechService.CreateBeautyTechAsync(created);
            if (result != null)
                await Shell.Current.DisplayAlert("Готово", "Мастер создан успешно", "OK");
        }
        else
        {
            beautyTech.Name = Name;
            beautyTech.PhoneNumber = PhoneNumber;
            beautyTech.Procedures = SelectedProcedureIds.ToList();

            var ok = await _beautyTechService.UpdateBeautyTechAsync(beautyTech);
            if (ok)
                await Shell.Current.DisplayAlert("Готово", "Мастер обновлён успешно", "OK");
        }

        await Shell.Current.GoToAsync("..");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Если ничего не передали — это Create режим, НЕ трогаем поля
        if (query == null || query.Count == 0)
            return;

        if (query.TryGetValue(nameof(BeautyTech), out var value) && value is BeautyTech b)
        {
            beautyTech = b;

            Name = b.Name;
            PhoneNumber = b.PhoneNumber;
            SelectedProcedureIds = new ObservableCollection<Guid>(b.Procedures);

            OnPropertyChanged(nameof(SelectedProceduresText));
            OnPropertyChanged(nameof(SelectedProcedures));
        }
    }

}
