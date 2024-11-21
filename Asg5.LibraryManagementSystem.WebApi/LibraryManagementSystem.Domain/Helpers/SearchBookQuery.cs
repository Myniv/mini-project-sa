using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Helpers
{
    public class SearchBookQuery
    {
        public string? Isbn { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }


        public string? SortBy { get; set; } = null;

        public string? SortOrder { get; set; } = "asc";

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
        public string? Keyword { get; set; } = null;
        // public string? AndOr1 { get; set; }
        // public string? AndOr2 { get; set; }
        // public string? AndOr3 { get; set; }

        // public string? SearchTerm { get; set; }


    }
}
