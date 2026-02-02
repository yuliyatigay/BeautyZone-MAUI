using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(IAuthService _authService)
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel(_authService);
    }
}