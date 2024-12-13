using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class LeaveRequestDto
    {
        public int RequestId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string File { get; set; }
        public string FileUrl { get; set; }
        public List<WorkflowActionDto> History { get; set; }
    }

    public class WorkflowActionDto
    {
        public DateTime? ActionDate { get; set; }
        public string Action { get; set; }
        public string Comments { get; set; }
    }

}
