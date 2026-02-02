using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Services;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IAuthService _authService;
    public AuthHeaderHandler(IAuthService authService)
    {
        _authService = authService;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_authService.UserSession is null)
            await _authService.FetchUserSession();

        var token = _authService.UserSession?.AccessToken;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
