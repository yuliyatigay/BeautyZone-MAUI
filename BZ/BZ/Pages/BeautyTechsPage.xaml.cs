using BZ.ViewModels;
using Domain.Interfaces;
using Domain.Models;

namespace BZ.Pages;

public partial class BeautyTechsPage : ContentPage
{
	private readonly BeautyTechViewModel viewModel;
	public BeautyTechsPage(IBeautyTechService beautyTechService, IProcedureService procedureService)
	{
		InitializeComponent();
		viewModel = new BeautyTechViewModel(beautyTechService, procedureService);
		BindingContext = viewModel;
    }
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ((BeautyTechViewModel)BindingContext).LoadAll();
    }
	private async void OnSpecialistSelected(object sender, SelectionChangedEventArgs e)
	{
		if (BindingContext is not BeautyTechViewModel vm)
			return;

		var specialist = e.CurrentSelection.FirstOrDefault() as BeautyTech;
		if (specialist == null)
			return;

		await vm.OpenBeautyTechPopupCommand.ExecuteAsync(specialist);

		((CollectionView)sender).SelectedItem = null;
	}
}