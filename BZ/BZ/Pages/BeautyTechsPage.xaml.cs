using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class BeautyTechsPage : ContentPage
{
	private readonly BeautyTechViewModel viewModel;
	public BeautyTechsPage(IBeautyTechService beautyTechService)
	{
		InitializeComponent();
		viewModel = new BeautyTechViewModel(beautyTechService);
		BindingContext = viewModel;
    }
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ((BeautyTechViewModel)BindingContext).LoadBeautyTechs();
    }
}