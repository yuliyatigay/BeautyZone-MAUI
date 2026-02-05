using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Domain.Interfaces;

public interface IProcedureService
{
    Task<ObservableCollection<Procedure>> GetAllProcedures(bool forceRefresh = false);
    Task Delete(Guid id);
    Task<Procedure?> CreateProcedure(string procedureName);
    Task<bool> UpdateProcedure(Procedure procedure);
}
