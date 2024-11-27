using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWeb.Domain.Models.Helpers
{
    public class SearchEmployeeQuery2
    {
        public string? Name { get; set; }

        public string? Fname { get; set; } = null!;
        public string? Lname { get; set; } = null!;
        public string? Position { get; set; } = null!;
        public int? EmpLevel { get; set; }
        public string? EmpType { get; set; } = null!;


        public string? KeyWord { get; set; }
        public string? SearchBy { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public bool? isActive { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";

    }
}
