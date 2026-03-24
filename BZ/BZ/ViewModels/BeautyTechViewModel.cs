using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;
using BZ.Pages.Popups;
using CommunityToolkit.Maui.Views;

namespace BZ.ViewModels
{
    public partial class BeautyTechViewModel : ObservableObject
    {
        private readonly IBeautyTechService _beautyTechService;

        [ObservableProperty]
        private ObservableCollection<BeautyTech> beautyTechs = new();
        public ObservableCollection<ProcedurePickerItem> ProcedurePickers { get; } = new();
        [ObservableProperty]
        private bool isRefreshing;
        [ObservableProperty]
        private bool isLoading;

        public BeautyTechViewModel(IBeautyTechService beautyTechService)
        {
            _beautyTechService = beautyTechService;
        }

        public async Task OpenBeautyTechPopup(Procedure procedure)
        {
            
        }

        public async Task LoadBeautyTechs()
        {
            try
            {
                IsLoading = true;
                var list = await _beautyTechService.GetAllBeautyTechs();
                BeautyTechs = new ObservableCollection<BeautyTech>(list);
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
                var list = await _beautyTechService.GetAllBeautyTechs(true);
                BeautyTechs = new ObservableCollection<BeautyTech>(list);
                IsRefreshing = false;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task Delete(Guid id)
        {
            await _beautyTechService.DeleteBeautyTechAsync(id);
            var deleted = BeautyTechs.FirstOrDefault(p => p.Id == id);

            if (deleted != null)
            {
                BeautyTechs.Remove(deleted);
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