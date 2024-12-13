using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class QueryObjectLeaveRequest
    {
        public int? Id { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? LeaveType { get; set; } = null;
        public string? CurrentStatus { get; set; } = null;
        public string? RequestDate { get; set; } = null;
        public string? Keyword { get; set; } = null;
        public string? SortBy { get; set; } = "requestId";
        public string? SortOrder { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
