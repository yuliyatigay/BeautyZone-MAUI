using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Domain.Interfaces;

public interface IProcedureService
{
    Task<ObservableCollection<Procedure>> GetAllProcedures();
}
