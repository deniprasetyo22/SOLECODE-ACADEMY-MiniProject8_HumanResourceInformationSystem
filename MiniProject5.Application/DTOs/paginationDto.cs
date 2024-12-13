using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class paginationDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? orderBy {  get; set; }
        public string? OrderByDirection {  get; set; }
    }
}
