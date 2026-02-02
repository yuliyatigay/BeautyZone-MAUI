using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IAuthService
{
    UserSession UserSession { get; }
    bool isAdmin { get; }
    Task FetchUserSession();
    Task<bool> Login(string email, string password);
    Task Logout();
}
