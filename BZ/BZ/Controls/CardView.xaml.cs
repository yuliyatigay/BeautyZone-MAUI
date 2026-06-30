using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZ.Controls;

public partial class CardView : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CardView));
    public static readonly BindableProperty PhoneNumberProperty =
        BindableProperty.Create(nameof(PhoneNumber), typeof(string), typeof(CardView));
    public static readonly BindableProperty CardDescriptionProperty =
        BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(CardView));
    public static readonly BindableProperty IconImageSourceProperty =
        BindableProperty.Create(nameof(IconImageSource), typeof(ImageSource ), typeof(CardView));
    public CardView()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public string PhoneNumber
    {
        get => GetValue(PhoneNumberProperty) as string;
        set => SetValue(PhoneNumberProperty, value);
    }
    public string CardDescription
    {
        get => GetValue(CardDescriptionProperty) as string;
        set => SetValue(CardDescriptionProperty, value);
    }
    public ImageSource IconImageSource
    {
        get => GetValue(IconImageSourceProperty) as ImageSource;
        set => SetValue(IconImageSourceProperty, value);
    }
}