using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class QueryObjectEmployee
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? FullName { get; set; } = null;
        public int? Department { get; set; } = null;
        public string? Position { get; set; } = null;
        public int? Level { get; set; } = null;
        public string? EmpType { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? LastUpdated { get; set; } = null;
        public string? Keyword { get; set; } = null;

        public string? SortBy { get; set; } = "empId";
        public string? SortOrder { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
