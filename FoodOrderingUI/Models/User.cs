﻿namespace FoodOrderingUI.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }

}
