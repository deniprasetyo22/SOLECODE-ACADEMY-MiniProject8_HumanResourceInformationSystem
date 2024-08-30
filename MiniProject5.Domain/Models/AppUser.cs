using Microsoft.AspNetCore.Identity;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Domain.Models
{
    public class AppUser:IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        // Navigation property to Employee
        public virtual Employee? Employee { get; set; }
    }
}
