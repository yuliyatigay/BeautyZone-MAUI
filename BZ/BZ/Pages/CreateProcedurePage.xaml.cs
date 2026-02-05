using BZ.ViewModels;
using Domain.Interfaces;

namespace BZ.Pages;

public partial class CreateProcedurePage : ContentPage
{

    public CreateProcedurePage(IProcedureService procedureService)
	{
        InitializeComponent();
        BindingContext = new ProcedureFormViewModel(procedureService);
    }

}