﻿using System.Globalization;
using WebApi.Common.Enums.Auth;

namespace WebApi.Application.ApplicationUsers
{
    public interface IApplicationUser
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public CultureInfo Locale { get; set; }
        public List<string> AllowedUseCases { get; set; }
    }
}
