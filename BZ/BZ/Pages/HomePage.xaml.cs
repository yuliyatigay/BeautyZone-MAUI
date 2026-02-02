using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel viewModel;
    public HomePage(IAuthService authService, IHttpClientFactory clientFactory)
    {
        InitializeComponent();
        viewModel = new HomePageViewModel(authService, clientFactory);
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.CheckAuthStatus();
    }
}