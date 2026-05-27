using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;

namespace BZ.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private bool isAdmin;

    public HomePageViewModel(IAuthService authService, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AppHttpClient");
        _authService = authService;
    }

    public async Task CheckAuthStatus()
    {
        await _authService.FetchUserSession();

        if (_authService.UserSession is null)
        {
            await Shell.Current.GoToAsync($"//LoginPage");
        }
        else
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Remove("Authorization");

            _httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {_authService.UserSession.AccessToken}");

            isAdmin = _authService.isAdmin;
        }
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _authService.Logout();
        await Shell.Current.GoToAsync($"//LoginPage");
    }
    [RelayCommand]
    private async Task NavigateToProceduresPage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(ProcedurePage));
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
        }
    }
    [RelayCommand]
    private async Task NavigateToBeautyTechPage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(BeautyTechsPage));
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
        }
    }
    [RelayCommand]
    private async Task NavigateToCustomerPage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(CustomerPage));
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
        }
    }
}
