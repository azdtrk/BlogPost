﻿using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Application.DTOs.User
{
    public class AuthenticatedUserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public TokenDto Token { get; set; }

        public UserRole Role { get; set; }
    }
}
