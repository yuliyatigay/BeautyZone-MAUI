using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class ProcedureFormPage : ContentPage
{

    public ProcedureFormPage(IProcedureService procedureService)
	{
        InitializeComponent();
        BindingContext = new ProcedureFormViewModel(procedureService);
    }

}