using BZ.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Interfaces;

namespace BZ.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;
    public LoginPageViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await Shell.Current.DisplayAlert("Login failed", "Invalid Email or password", "OK");
            return;
        }

        var isLoggedIn = await _authService.Login(Email, Password);

        if (isLoggedIn)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
            await Shell.Current.DisplayAlert("Login failed", "Invalid Email or password", "OK");
        }
    }

}