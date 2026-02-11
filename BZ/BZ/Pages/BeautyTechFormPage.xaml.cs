using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class BeautyTechFormPage : ContentPage
{
    public BeautyTechFormPage(BeautyTechFormViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((BeautyTechFormViewModel)BindingContext).LoadProcedures();
    }
}