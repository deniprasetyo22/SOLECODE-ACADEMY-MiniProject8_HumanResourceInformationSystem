using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class ProcessDto
    {
        public int Processid { get; set; }
        public int? Workflowid { get; set; }
        public string? RequesterUsername { get; set; }
        public DateTime? Requestdate { get; set; }
        public string? Status { get; set; }
        public string? CurrentstepName { get; set; }
    }
}
