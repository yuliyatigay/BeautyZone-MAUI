using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private ObservableCollection<Customer> _cache;
    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("HttpClient");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public Task<HttpResponseMessage> GetAsync(string url, CancellationToken ct = default)
        => _httpClient.GetAsync(url, ct);
    
     public async Task<ObservableCollection<Customer>> GetAllCustomers(bool forceRefresh = false)
    {
        if (_cache != null && !forceRefresh)
            return _cache;

        using var response = await _httpClient.GetAsync("api/Customer/GetAllCustomers");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return new ObservableCollection<Customer>();

        response.EnsureSuccessStatusCode();

        _cache = await response.Content.ReadFromJsonAsync<ObservableCollection<Customer>>(_options)
                 ?? new ObservableCollection<Customer>();

        return _cache;
    }

    public async Task Delete(Guid id)
    {
        using var response = await _httpClient.DeleteAsync($"api/Customer/DeleteCustomer/{id}");
        response.EnsureSuccessStatusCode();
        var item = _cache?.FirstOrDefault(x => x.Id == id);
        if (item != null) _cache.Remove(item);
    }

    public async Task<Customer?> CreateCustomer(Customer customer)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/Customer/CreateCustomer",
            new
            {
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber
            });
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            await Shell.Current.DisplayAlert("Ошибка",
                "Клиент уже есть в базе данных", "OK");
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Ошибка", body, "OK");
            return null;
        }


        var created = await response.Content.ReadFromJsonAsync<Customer>(_options);
        if (created == null) return null;

        _cache ??= new ObservableCollection<Customer>();
        _cache.Add(created);

        return created;
    }

    public async Task<bool> UpdateCustomer(Customer customer)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/Customer/UpdateCustomer/{customer.Id}", new
        {
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber
        });
        if (!response.IsSuccessStatusCode)                  
            return false;
        var cached = _cache?.FirstOrDefault(x => x.Id == customer.Id);
        if (cached != null)
            cached.Name = customer.Name;
            cached.PhoneNumber = customer.PhoneNumber;

        return true;
    }
}