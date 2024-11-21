using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Models.Responses
{
    public class BookSearchResponse
    {
        public int BookId { get; set; }
        public string? Author { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int Price { get; set; }
        public string[]? Locations { get; set; } = null!;
    }
}
