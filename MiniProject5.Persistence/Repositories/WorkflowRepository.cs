using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.Interfaces.IRepositories;
using MiniProject6.Domain.Models;
using MiniProject6.Persistence.Models;
using MiniProject7.Application.Interfaces.IServices;
using MiniProject7.Domain.Models.Email;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Persistence.Repositories
{
    public class WorkflowRepository:IWorkflowRepository
    {
        private readonly HrisContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public WorkflowRepository(HrisContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request)
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            var userName = currentUser?.UserName;

            var userEmail = currentUser?.Email;

            var process = new Process
            {
                Workflowid = 1,
                Requestdate = DateTime.Now,
                Status = "Supervisor Review Leave Request",
                Currentstepid = 2,
                Requesttype = "Leave Request",
                Requesterid = userId
            };
            await _context.Processes.AddAsync(process);
            await _context.SaveChangesAsync();

            //workflowaction
            var workflow = new Workflowaction
            {
                Processid = process.Processid,
                Stepid = 1,
                Actorid = userId,
                Action = process.Status,
                Actiondate = DateTime.Now,
                Comments = "Submitted a leave request"
            };

            await _context.Workflowactions.AddAsync(workflow);
            await _context.SaveChangesAsync();

            request.Empid = request.Empid;
            request.Processid = process.Processid;
            request.Startdate = DateTime.Now;
            request.Enddate = request.Startdate?.AddDays(7);

            await _context.Leaverequests.AddAsync(request);
            await _context.SaveChangesAsync();

            var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/LeaveRequest.html");
            emailBody = string.Format(emailBody,
                "Leave Request",    //{0}
                userName            //{1}
            );

            var mailData = new MailData
            {
                EmailToIds = new List<string> { userEmail },
                EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                EmailToName = userName,
                EmailSubject = "Welcome to Our Service!",
                EmailBody = emailBody
            };

            var emailSent = _emailService.SendMail(mailData);

            return request;
        }

        public async Task ApproveOrRejectLeaveRequestAsync(int processId, Process Process)
        {
            var userRoles = _httpContextAccessor.HttpContext?.User.Claims
                  .Where(c => c.Type == ClaimTypes.Role)
                  .Select(c => c.Value)
                  .ToList();

            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            var userName = currentUser?.UserName;

            var userEmail = currentUser?.Email;

            var existingProcess = await _context.Processes.Include(p => p.Requester).FirstOrDefaultAsync(cek => cek.Processid == processId);

            if (userRoles.Contains("Employee Supervisor"))
            {
                if (existingProcess != null)
                {
                    if (Process.Status == "Approve")
                    {
                        existingProcess.Currentstepid = 3;

                        existingProcess.Status = "HR Manager Review Leave Request";

                        var workflow = new Workflowaction
                        {
                            Processid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Approved",
                            Actiondate = DateTime.Now,
                            Comments = "Your Leave Request Approved By Supervisor"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApproveOrRejectLeaveRequest.html");
                        emailBody = string.Format(emailBody,
                            "Leave Request",                    //{0}
                            existingProcess.Requester.UserName,    //{1}
                            "Approved",                         //{2}
                            userName                            //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    if (Process.Status == "Reject")
                    {
                        existingProcess.Currentstepid = 5;

                        existingProcess.Status = "Request Rejected";

                        var workflow = new Workflowaction
                        {
                            Processid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Rejected",
                            Actiondate = DateTime.Now,
                            Comments = "Your Leave Request Rejected By Supervisor"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApproveOrRejectLeaveRequest.html");
                        emailBody = string.Format(emailBody,
                            "Leave Request",    //{0}
                            existingProcess.Requester.UserName,           //{1}
                            "Rejected",         //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            if (userRoles.Contains("HR Manager"))
            {
                if (existingProcess != null)
                {
                    if (Process.Status == "Approve")
                    {
                        existingProcess.Currentstepid = 5;

                        existingProcess.Status = "Request Approved";

                        var workflow = new Workflowaction
                        {
                            Processid = processId,
                            Stepid = 3,
                            Actorid = userId,
                            Action = "Approved",
                            Actiondate = DateTime.Now,
                            Comments = "Your Leave Request Approved By HR Manager"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApproveOrRejectLeaveRequest.html");
                        emailBody = string.Format(emailBody,
                            "Leave Request",    //{0}
                            existingProcess.Requester.UserName,           //{1}
                            "Approved",         //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    if (Process.Status == "Reject")
                    {
                        existingProcess.Currentstepid = 4;

                        existingProcess.Status = "Request Rejected";

                        var workflow = new Workflowaction
                        {
                            Processid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Rejected",
                            Actiondate = DateTime.Now,
                            Comments = "Your Leave Request Rejected By HR Manager"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApproveOrRejectLeaveRequest.html");
                        emailBody = string.Format(emailBody,
                            "Leave Request",    //{0}
                            existingProcess.Requester.UserName,           //{1}
                            "Rejected",         //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }
                    await _context.SaveChangesAsync();
                }

            }

        }

        //Workflow Process
        public async Task<List<ProcessDto>> GetAllProcessesAsync()
        {
            var userRoles = _httpContextAccessor.HttpContext?.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var processes = await _context.Processes
                .Include(p => p.Currentstep) // Sertakan Currentstep
                .Where(p => p.Currentstep != null && userRoles.Contains(p.Currentstep.Requiredrole))
                .Select(p => new ProcessDto
                {
                    Processid = p.Processid,
                    Workflowid = p.Workflowid,
                    RequesterUsername = p.Requester.UserName,
                    Requestdate = p.Requestdate,
                    Status = p.Status,
                    CurrentstepName = p.Currentstep.Stepname
                })
                .ToListAsync();
            return processes;
        }
    }
}
