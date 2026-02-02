using Domain.Interfaces;
using Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DataAccess.Services;

public class AuthService : IAuthService
{
    private const string USER_SESSION_STORAGE_KEY = "user_session";
    private readonly HttpClient _httpClient;
    private UserSession? _userSession;
    private readonly JsonSerializerOptions _options;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AppHttpClient");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public UserSession? UserSession => _userSession;
    public bool isAdmin => _userSession is not null &&
                           _userSession.Role == UserRole.admin;



    public async Task FetchUserSession()
    {
        var userSessionJson = await SecureStorage.Default.GetAsync(USER_SESSION_STORAGE_KEY);
        if (!string.IsNullOrWhiteSpace(userSessionJson))
        {
            _userSession = JsonSerializer.Deserialize<UserSession>(userSessionJson, _options);
        }
    }

    public async Task<bool> Login(string email, string password)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/Account/Login", new
        {
            Email = email,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
            return false;

        var stream = await response.Content.ReadAsStreamAsync();
        _userSession = await JsonSerializer.DeserializeAsync<UserSession>(stream, _options);

        if (_userSession is null)
            return false;

        await SecureStorage.Default.SetAsync(
            USER_SESSION_STORAGE_KEY,
            JsonSerializer.Serialize(_userSession, _options)
        );
        return true;
    }

    public async Task Logout()
    {
        SecureStorage.Remove(USER_SESSION_STORAGE_KEY);
        _userSession = null;
    }
}
