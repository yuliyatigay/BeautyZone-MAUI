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

    public async Task<ObservableCollection<Procedure>> GetAllProcedures()
    {
        using var response = await _httpClient.GetAsync("api/Procedure/GetAllProcedures");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new ObservableCollection<Procedure>();
        }
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ObservableCollection<Procedure>>(_options);
    }

    public async Task DeleteProcedure()
    {
        using var response = await _httpClient.DeleteAsync($"api/Procedure/DeleteProcedure");
        response.EnsureSuccessStatusCode();
    }
}
