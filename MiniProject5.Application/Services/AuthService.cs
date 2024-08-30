using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs.Account;
using MiniProject6.Application.Interfaces.IServices;
using MiniProject6.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmployeeRepository employeeRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
        }
        //Sign Up The User
        public async Task<ResponseModel> SignUpAsync(RegisterModel model)
        {
            // Check if the user already exists
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new ResponseModel { Status = "Error", Message = "User already exists!" };
            }

            // Create AppUser
            AppUser user = new AppUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            // Create the user in AspNetUsers
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (!userResult.Succeeded)
            {
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                };
            }

            // Ensure the AppUser has been fully committed to the database
            await _userManager.UpdateAsync(user);

            // Create Employee linked to AppUser
            Employee newEmployee = new Employee
            {
                Fname = model.FirstName,
                Lname = model.LastName,
                Ssn = model.Ssn,
                Email = model.Email,
                Address = model.Address,
                Position = model.Position,
                Salary = model.Salary,
                Sex = model.Sex,
                Dob = model.Dob,
                Phoneno = model.Phoneno,
                Emptype = model.Emptype,
                Level = model.Level,
                Deptid = model.Deptid,
                Status = "Active",
                Lastupdateddate = DateTime.Now,
                SupervisorId = model.SupervisorId,
                userId = user.Id
            };

            // Add Dependents to Employee
            foreach (var dependentModel in model.Dependents)
            {
                newEmployee.Dependents.Add(new Dependent
                {
                    fName = dependentModel.fName,
                    lName = dependentModel.lName,
                    Sex = dependentModel.Sex,
                    Dob = dependentModel.Dob,
                    Relationship = dependentModel.Relationship
                });
            }

            // Add Employee to the database
            try
            {
                var createdEmployee = await _employeeRepository.AddEmployeeAsync(newEmployee);

                // Link the Employee to the AppUser explicitly if needed
                user.Employee = createdEmployee;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                // Rollback AppUser creation if employee creation fails
                await _userManager.DeleteAsync(user);

                return new ResponseModel
                {
                    Status = "Error",
                    Message = $"Employee creation failed! Error: {ex.Message}"
                };
            }

            var roleName = model.Role; // or any other role as per requirement
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "Role assignment failed! Please check the role and try again."
                };
            }

            return new ResponseModel { Status = "Success", Message = "User, Employee, Dependents, and Role assigned successfully!" };
        }



        //Login user
        public async Task<ResponseModel> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                return new ResponseModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiredOn = token.ValidTo,
                    Message = "User successfully login!",
                    RefreshToken = refreshToken,
                    Roles = userRoles.ToList(),
                    Status = "Success"
                };
            }
            return new ResponseModel { Status = "Error", Message = "Password Not valid!" };

        }
        // Create Role
        public async Task<ResponseModel> CreateRoleAsync(string rolename)
        {
            if (!await _roleManager.RoleExistsAsync(rolename))
                await _roleManager.CreateAsync(new IdentityRole(rolename));
            return new ResponseModel { Status = "Success", Message = "Role Created successfully!" };
        }

        // Assign user to role that already created before
        public async Task<ResponseModel> AssignToRoleAsync(string userName, string rolename)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (await _roleManager.RoleExistsAsync($"{rolename}"))
            {
                await _userManager.AddToRoleAsync(user, rolename);
            }
            return new ResponseModel { Status = "Success", Message = "User created succesfully!" };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<ResponseModel> LogoutAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new ResponseModel { Status = "Error", Message = "User not found!" };
            }

            // Invalidate the user's refresh token
            user.RefreshToken = null;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResponseModel { Status = "Success", Message = "User successfully logged out!" };
            }

            return new ResponseModel { Status = "Error", Message = "Logout failed! Please try again." };
        }
    }
}
