﻿namespace CA.Application.DTOs.Identity.Requests
{
    public class UpdateProfileRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}