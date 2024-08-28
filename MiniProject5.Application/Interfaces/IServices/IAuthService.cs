using MiniProject6.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ResponseModel> SignUpAsync(RegisterModel model);
        Task<ResponseModel> LoginAsync(LoginModel model);
        Task<ResponseModel> CreateRoleAsync(string rolename);
        Task<ResponseModel> AssignToRoleAsync(string userName, string rolename);
        string GenerateRefreshToken();
        Task<ResponseModel> LogoutAsync(string username);
    }
}
