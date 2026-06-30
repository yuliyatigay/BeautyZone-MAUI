using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models;

public class UserSession
{
    public string? Email { get; set; }
    public string? AccessToken { get; set; }
    public DateTime ExpiryTime { get; set; }
    public UserRole Role { get; set; }
}
