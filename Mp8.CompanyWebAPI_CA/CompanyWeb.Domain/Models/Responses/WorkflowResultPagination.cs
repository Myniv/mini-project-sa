using CompanyWeb.Domain.Models.Responses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Models.Responses
{
    public class WorkflowResultPagination
    {
        public int ProcessId { get; set; }
        public string? Requester { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Status { get; set; }
        public string? LeaveType { get; set; }
        public string? LeaveReason { get; set; }
        // public string? CurrentStep { get; set; }
        // public object? LeaveRequest { get; set; } // Adjust the type of LeaveRequest if necessary
    }
}
