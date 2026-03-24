using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DataAccess.Services
{
    public class BeautyTechService : IBeautyTechService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private ObservableCollection<BeautyTech> _cache;
        public BeautyTechService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HttpClient");
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public Task<HttpResponseMessage> GetAsync(string url, CancellationToken ct = default)
        => _httpClient.GetAsync(url, ct);

        public async Task<ObservableCollection<BeautyTech>> GetAllBeautyTechs(bool forceRefresh = false)
        {
            if (_cache != null && !forceRefresh)
                return _cache;

            using var response = await _httpClient.GetAsync("api/BeautyTech/FetchAllBeautyTechs");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new ObservableCollection<BeautyTech>();

            response.EnsureSuccessStatusCode();

            _cache = await response.Content.ReadFromJsonAsync<ObservableCollection<BeautyTech>>(_options)
                     ?? new ObservableCollection<BeautyTech>();

            return _cache;
        }

        public async Task<BeautyTech?> CreateBeautyTechAsync(BeautyTech beautyTech)
        {
            using var response = await _httpClient.PostAsJsonAsync(
            "api/BeautyTech/AddBeautyTech",new 
            {
                Name = beautyTech.Name,
                PhoneNumber = beautyTech.PhoneNumber,
                Procedures = beautyTech.Procedures.Select(x => x.Id)
            });
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    "Нельзя добавить существующего мастера", "OK");
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                await Shell.Current.DisplayAlert("Ошибка", body, "OK");
                return null;
            }
            var created = await response.Content.ReadFromJsonAsync<BeautyTech>(_options);
            if (created == null) return null;

            _cache ??= new ObservableCollection<BeautyTech>();
            _cache.Add(created);

            return created;
        }

        public async Task DeleteBeautyTechAsync(Guid id)
        {
            using var response = await _httpClient.DeleteAsync($"api/BeautyTech/DeleteBeautyTechAsync/{id}");
            response.EnsureSuccessStatusCode();
            var item = _cache?.FirstOrDefault(x => x.Id == id);
            if (item != null) _cache.Remove(item);
        }

        

        public async Task<BeautyTech> GetBeautyTechById(Guid id)
        {
            using var response = await _httpClient.GetAsync($"api/BeautyTech/GetBeautyTechById/{id}");
            response.EnsureSuccessStatusCode();
            var beautyTech = await response.Content.ReadFromJsonAsync<BeautyTech>(_options);
            return beautyTech!;
        }

        public async Task<bool> UpdateBeautyTechAsync(BeautyTech beautyTech)
        {
            using var response = await _httpClient.PutAsJsonAsync($"api/BeautyTech/UpdateBeautyTechAsync/{beautyTech.Id}", new
            {
                Name = beautyTech.Name,
                PhoneNumber = beautyTech.PhoneNumber,
                Procedures = beautyTech.Procedures
            });
            if (!response.IsSuccessStatusCode)
                return false;
            var cached = _cache?.FirstOrDefault(x => x.Id == beautyTech.Id);
            if (cached != null) 
            {
                cached.Name = beautyTech.Name;
                cached.PhoneNumber = beautyTech.PhoneNumber;
                cached.Procedures = beautyTech.Procedures;
            }
            
            return true;
        }
    }
}
