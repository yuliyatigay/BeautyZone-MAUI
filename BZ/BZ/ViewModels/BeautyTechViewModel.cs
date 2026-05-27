using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;
using BZ.Pages.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace BZ.ViewModels
{
    public partial class BeautyTechViewModel : ObservableObject
    {
        private readonly IBeautyTechService _beautyTechService;
        private readonly IProcedureService _procedureService;

        [ObservableProperty]
        private ObservableCollection<BeautyTech> specialists = new();

        [ObservableProperty] 
        private ObservableCollection<Procedure> procedures = new();
        [ObservableProperty]
        private BeautyTech newSpecialist = new();
        public ObservableCollection<ProcedurePickerItem> ProcedurePickers { get; } = new();
        [ObservableProperty]
        private bool isRefreshing;
        [ObservableProperty]
        private bool isLoading;

        public BeautyTechViewModel(IBeautyTechService beautyTechService, IProcedureService procedureService)
        {
            _beautyTechService = beautyTechService;
            _procedureService = procedureService;
            AddProcedurePicker();
        }
        public async Task LoadAll()
        {
            try
            {
                IsLoading = true;
                var allProcedures = await _procedureService.GetAllProcedures();
                var allSpecialist = await _beautyTechService.GetAllBeautyTechs();
                Procedures = new ObservableCollection<Procedure>(allProcedures);
                Specialists = new ObservableCollection<BeautyTech>(allSpecialist);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task OpenBeautyTechPopup(BeautyTech beautyTech)
        {
            var popup = new BeautyTechFormPopup(beautyTech, Procedures, async edited =>Submit(edited));
            
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }
        
        [RelayCommand]
        
        public async Task Submit(BeautyTech specialist)
        {
        
            if (specialist.Id == Guid.Empty)
            {
                var result = await _beautyTechService.CreateBeautyTechAsync(specialist);
                if(result != null)
                {
                    await Shell.Current.DisplayAlert("Готово", "Мастер создан успешно", "OK");
                }

                await Refresh();
            }
            else
            {
                var result = await _beautyTechService.UpdateBeautyTechAsync(specialist);
                if (result)
                {
                    await Shell.Current.DisplayAlert("Готово", "Мастер обновлен успешно", "OK");
                }
                await Refresh();
            }
        }
        [RelayCommand]
        private void AddProcedurePicker()
        {
            ProcedurePickers.Add(new ProcedurePickerItem());
        }
        [RelayCommand]
        private void RemoveProcedurePicker(ProcedurePickerItem item)
        {
            if (item is null)
                return;

            ProcedurePickers.Remove(item);
        }

        [RelayCommand]
        public async Task Refresh()
        {
            try
            {
                IsRefreshing = true;
                var list = await _beautyTechService.GetAllBeautyTechs(true);
                Specialists = new ObservableCollection<BeautyTech>(list);
                IsRefreshing = false;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        
        [RelayCommand]
        public async Task Delete(BeautyTech specialist)
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Удаление",
                "Вы действительно хотите удалить эту процедуру?",
                "Да",
                "Нет");

            if (!confirm)
                return;
            
            await _beautyTechService.DeleteBeautyTechAsync(specialist.Id);

            if (specialist != null)
            {
                Specialists.Remove(specialist);
                await Shell.Current.DisplayAlert("Готово", "Мастер удален успешно", "OK");
            }

        }
    }
}
public partial class ProcedurePickerItem : ObservableObject
{
    [ObservableProperty]
    private Procedure? selectedProcedure;
}