using Domain.Interfaces;
using Domain.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DataAccess.Services;

public class ProcedureService : IProcedureService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private ObservableCollection<Procedure> _cache;
    public ProcedureService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("HttpClient");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public Task<HttpResponseMessage> GetAsync(string url, CancellationToken ct = default)
        => _httpClient.GetAsync(url, ct);

    public async Task<ObservableCollection<Procedure>> GetAllProcedures(bool forceRefresh = false)
    {
        if (_cache != null && !forceRefresh)
            return _cache;

        using var response = await _httpClient.GetAsync("api/Procedure/GetAllProcedures");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return new ObservableCollection<Procedure>();

        response.EnsureSuccessStatusCode();

        _cache = await response.Content.ReadFromJsonAsync<ObservableCollection<Procedure>>(_options)
                 ?? new ObservableCollection<Procedure>();

        return _cache;
    }

    public async Task Delete(Guid id)
    {
        using var response = await _httpClient.DeleteAsync($"api/Procedure/DeleteProcedure/{id}");
        response.EnsureSuccessStatusCode();
        var item = _cache?.FirstOrDefault(x => x.Id == id);
        if (item != null) _cache.Remove(item);
    }

    public async Task<Procedure?> CreateProcedure(string procedureName)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/Procedure/CreateProcedure",
            new { Name = procedureName });
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            await Shell.Current.DisplayAlert("Ошибка",
                "Нельзя добавить существующую процедуру", "OK");
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Ошибка", body, "OK");
            return null;
        }


        var created = await response.Content.ReadFromJsonAsync<Procedure>(_options);
        if (created == null) return null;

        _cache ??= new ObservableCollection<Procedure>();
        _cache.Add(created);

        return created;
    }

    public async Task<bool> UpdateProcedure(Procedure procedure)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/Procedure/UpdateProcedure/{procedure.Id}", new
        {
            Name = procedure.Name
        });
        if (!response.IsSuccessStatusCode)                  
            return false;
        var cached = _cache?.FirstOrDefault(x => x.Id == procedure.Id);
        if (cached != null) cached.Name = procedure.Name;

        return true;
    }
}
