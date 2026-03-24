using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Domain.Models;

namespace BZ.Pages.Popups;

public partial class BeautyTechInfo : Popup
{
    public BeautyTech Specialist {get; }
    public BeautyTechInfo()
    {
        InitializeComponent();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDoneClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}