using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<ObservableCollection<Customer>> GetAllCustomers(bool forceRefresh = false);
        Task Delete(Guid id);
        Task<Customer?> CreateCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
    }
}
