using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Domain.Models.Entities;

namespace LibraryManagementSystem.Domain.Models.Responses
{
    public class AppUserResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public DateTime? RefreshTokenExpiredOn { get; set; }

        public AppUser? User { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

        public List<string> Role { get; set; } = null!;

        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? Email { get; set; }


    }
}
