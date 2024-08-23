using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.DTOs
{
    public class searchDto
    {
        public int? empId {  get; set; }
        public string? fName {  get; set; }
        public string? lName { get; set; }
        public string? ssn {  get; set; }
        public string? address {  get; set; }
        public string? position {  get; set; }
        public string? sex { get; set; }
        public string? empType {  get; set; }
        public int? level {  get; set; }
        public int? deptId {  get; set; }
        public string? status {  get; set; }
    }
}
