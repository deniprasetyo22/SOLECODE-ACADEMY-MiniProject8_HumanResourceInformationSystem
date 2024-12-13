using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IRepositories;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models.Email;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiniProject8.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProcessRepository _processRepository;
        private readonly IWorkflowActionRepository _workflowActionRepository;
        private readonly EmailService _emailService;
        private readonly IWorkflowRepository _workflowRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager,
            IProcessRepository process,
            IWorkflowActionRepository workflowActionRepository,
            EmailService emailService,
            IWorkflowRepository workflowRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _processRepository = process;
            _workflowActionRepository = workflowActionRepository;
            _emailService = emailService;
            _workflowRepository = workflowRepository;
        }

        public async Task<Leaverequest> AddLeaveRequestAsync(Leaverequest leaveRequest, IFormFile? file)
        {
            // Ensure the user is authenticated
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            // Retrieve the current user
            var currentUser = await _userManager.Users.Include(x => x.Employee).FirstOrDefaultAsync(x => x.UserName == userName);
            if (currentUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var userId = currentUser.Id;

            // Check if the workflow exists
            var workflowExists = await _workflowRepository.GetWorkflowByIdAsync(1); // Adjust the ID as necessary
            if (workflowExists == null)
            {
                throw new InvalidOperationException("The specified workflow does not exist.");
            }

            // Create a new Process
            var process = new Process
            {
                Workflowid = 1,
                Requestdate = DateTime.Now,
                Status = "Reviewed By Supervisor",
                Currentstepid = 2,
                Requesttype = "Leave Request",
                Requesterid = userId
            };

            // Add the process to the repository
            await _processRepository.AddProcessAsync(process);

            // Parse StartDate and EndDate without time
            if (DateTime.TryParse(leaveRequest.Startdate.ToString(), out DateTime startDate))
            {
                leaveRequest.Startdate = startDate.Date; // Set only the date part
            }

            if (DateTime.TryParse(leaveRequest.Enddate.ToString(), out DateTime endDate))
            {
                leaveRequest.Enddate = endDate.Date; // Set only the date part
            }

            // Calculate TotalDays
            if (leaveRequest.Startdate.HasValue && leaveRequest.Enddate.HasValue)
            {
                // Calculate total days including both start and end dates
                leaveRequest.Totaldays = (leaveRequest.Enddate.Value - leaveRequest.Startdate.Value).Days + 1;
            }

            // Validate file if TotalDays > 1 and Leavetype is "Sick Leave"
            if (leaveRequest.Totaldays > 1 && leaveRequest.Leavetype == "Sick Leave" || file != null && file.Length > 0)
            {
                if (file == null || (file.ContentType != "application/pdf" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg"))
                {
                    throw new InvalidOperationException("Sick Leave more than a day medical certificate is required and must be a PDF or JPEG image.");
                }

                // Check file size (max 5MB)
                const long maxFileSize = 5 * 1024 * 1024; // 5MB in bytes
                if (file.Length > maxFileSize)
                {
                    throw new InvalidOperationException("File size must not exceed 5MB.");
                }

                // Define the path to save the file
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");

                // Create the folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name using GUID
                var fileExtension = Path.GetExtension(file.FileName); // Get the file extension
                var fileName = Guid.NewGuid().ToString() + fileExtension; // Create a new file name with GUID
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                leaveRequest.File = fileName;
            }

            // Create a new Workflowaction
            var workflow = new Workflowaction
            {
                Processid = process.Processid,
                Stepid = 1,
                Actorid = userId,
                Action = process.Status,
                Actiondate = DateTime.Now,
                Comments = "Reviewed By Supervisor"
            };

            // Add the workflow action to the repository
            await _workflowActionRepository.AddWorkflowActionAsync(workflow);

            // Prepare the Leave Request entity
            leaveRequest.Empid = currentUser.Employee.Empid;
            leaveRequest.Processid = process.Processid; // Set the Process ID
            //leaveRequest.Startdate = DateTime.Now;
            //leaveRequest.Enddate = leaveRequest.Startdate?.AddDays(7);
            leaveRequest.Requestname = "Leave Request";
            leaveRequest.Description = "Leave Request";

            // Add the leave request to the repository
            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);

            // Prepare and send the email notification
            var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/LeaveRequest.html");
            emailBody = string.Format(emailBody, "Leave Request", currentUser.UserName);

            var mailData = new MailData
            {
                EmailToIds = new List<string> { currentUser.Email },
                EmailCCIds = new List<string> { "deni.prasetyo@yopmail.com" },
                EmailToName = currentUser.UserName,
                EmailSubject = "Welcome to Our Service!",
                EmailBody = emailBody
            };

            _emailService.SendMail(mailData);

            return leaveRequest;
        }
        public async Task ApprovalAsync(int processId, Process process)
        {
            var userRoles = _httpContextAccessor.HttpContext?.User.Claims
                  .Where(c => c.Type == ClaimTypes.Role)
                  .Select(c => c.Value)
                  .ToList();

            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);
            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            var userId = currentUser?.Id;
            var userName = currentUser?.UserName;
            var userEmail = currentUser?.Email;

            // Retrieve the existing process
            var existingProcess = await _processRepository.GetProcessByIdAsync(processId);
            if (existingProcess == null)
            {
                throw new InvalidOperationException($"Process with ID {processId} not found.");
            }

            if (userRoles.Contains("Employee Supervisor"))
            {
                existingProcess.Status = process.Status;
                if (process.Status == "Approve")
                {
                    existingProcess.Currentstepid = 3;
                    existingProcess.Status = "Reviewed By HR Manager";

                    var workflow = new Workflowaction
                    {
                        Processid = processId,
                        Stepid = 2,
                        Actorid = userId,
                        Action = existingProcess.Status,
                        Actiondate = DateTime.Now,
                        Comments = "Approved By Supervisor"
                    };

                    await _workflowActionRepository.AddWorkflowActionAsync(workflow);

                    var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApprovalLeaveRequest.html");
                    emailBody = string.Format(emailBody,
                        "Leave Request",           //{0}
                        existingProcess.Requester.UserName, //{1}
                        "Approved",                         //{2}
                        userName                            //{3}
                    );

                    var mailData = new MailData
                    {
                        EmailToIds = new List<string> { existingProcess.Requester.Email },
                        EmailCCIds = new List<string> { "hris.supervisor@yopmail.com" },
                        EmailToName = existingProcess.Requester.UserName,
                        EmailSubject = "Welcome to Our Service!",
                        EmailBody = emailBody
                    };

                    _emailService.SendMail(mailData);
                }
                else if (process.Status == "Reject")
                {
                    existingProcess.Currentstepid = 5;
                    existingProcess.Status = "Rejected";

                    var workflow = new Workflowaction
                    {
                        Processid = processId,
                        Stepid = 2,
                        Actorid = userId,
                        Action = existingProcess.Status,
                        Actiondate = DateTime.Now,
                        Comments = "Rejected By Supervisor"
                    };

                    await _workflowActionRepository.AddWorkflowActionAsync(workflow);

                    var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApprovalLeaveRequest.html");
                    emailBody = string.Format(emailBody,
                        "Leave Request",               //{0}
                        existingProcess.Requester.UserName,     //{1}
                        "Rejected",        //{2}
                        userName           //{3}
                    );

                    var mailData = new MailData
                    {
                        EmailToIds = new List<string> { existingProcess.Requester.Email },
                        EmailCCIds = new List<string> { "hris.supervisor@yopmail.com" },
                        EmailToName = existingProcess.Requester.UserName,
                        EmailSubject = "Welcome to Our Service!",
                        EmailBody = emailBody
                    };

                    try
                    {
                        _emailService.SendMail(mailData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
            else if (userRoles.Contains("HR Manager"))
            {
                existingProcess.Status = process.Status;
                if (process.Status == "Approve")
                {
                    existingProcess.Currentstepid = 5;
                    existingProcess.Status = "Approved";

                    var workflow = new Workflowaction
                    {
                        Processid = processId,
                        Stepid = 3,
                        Actorid = userId,
                        Action = existingProcess.Status,
                        Actiondate = DateTime.Now,
                        Comments = "Approved By HR Manager"
                    };

                    await _workflowActionRepository.AddWorkflowActionAsync(workflow);

                    var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApprovalLeaveRequest.html");
                    emailBody = string.Format(emailBody,
                        "Leave Request",               //{0}
                        existingProcess.Requester.UserName,     //{1}
                        "Approved",        //{2}
                        userName           //{3}
                    );

                    var mailData = new MailData
                    {
                        EmailToIds = new List<string> { existingProcess.Requester.Email },
                        EmailCCIds = new List<string> { "hris.hrmanager@yopmail.com" },
                        EmailToName = existingProcess.Requester.UserName,
                        EmailSubject = "Welcome to Our Service!",
                        EmailBody = emailBody
                    };

                    try
                    {
                        _emailService.SendMail(mailData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        // Optionally, you can throw an exception or log it
                    }
                }
                else if (process.Status == "Reject")
                {
                    existingProcess.Currentstepid = 5;
                    existingProcess.Status = "Rejected";

                    var workflow = new Workflowaction
                    {
                        Processid = processId,
                        Stepid = 2,
                        Actorid = userId,
                        Action = existingProcess.Status,
                        Actiondate = DateTime.Now,
                        Comments = "Rejected"
                    };

                    await _workflowActionRepository.AddWorkflowActionAsync(workflow);

                    var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplates/ApprovalLeaveRequest.html");
                    emailBody = string.Format(emailBody,
                        "Leave Request",               //{0}
                        existingProcess.Requester.UserName,     //{1}
                        "Rejected",        //{2}
                        userName           //{3}
                    );

                    var mailData = new MailData
                    {
                        EmailToIds = new List<string> { existingProcess.Requester.Email },
                        EmailCCIds = new List<string> { "hris.hrmanager@yopmail.com" },
                        EmailToName = existingProcess.Requester.UserName,
                        EmailSubject = "Welcome to Our Service!",
                        EmailBody = emailBody
                    };

                    try
                    {
                        _emailService.SendMail(mailData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        // Optionally, you can throw an exception or log it
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("User does not have the required role to approve or reject the request.");
            }
        }
        public async Task<object> GetAllLeaveRequestsAsync()
        {
            var leaveRequests = _leaveRequestRepository.GetAllLeaveRequestsAsync();

            var request = _httpContextAccessor.HttpContext.Request;

            var leaveRequestList = leaveRequests.Select(leaveRequest => new
            {
                RequestId = leaveRequest.Requestid,
                UserId = leaveRequest.Emp.userId,
                Name = leaveRequest.Emp.Fname + " " + leaveRequest.Emp.Lname,
                LeaveType = leaveRequest.Leavetype,
                StartDate = leaveRequest.Startdate,
                EndDate = leaveRequest.Enddate,
                TotalDays = leaveRequest.Totaldays,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Process.Status,
                RequestDate = leaveRequest.Process.Requestdate,
                File = leaveRequest.File,
                FileUrl = $"{request.Scheme}://{request.Host}/files/{leaveRequest.File}"
            }).ToList();

            var count = leaveRequestList.Count;

            return new
            {
                Count = count,
                LeaveRequests = leaveRequestList
            };
        }

        public async Task<object> GetLeaveRequestByIdAsync(int requestId)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(requestId);

            if (leaveRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveRequest));
            }

            var request = _httpContextAccessor.HttpContext.Request;

            return new
            {
                RequestId = leaveRequest.Requestid,
                UserId = leaveRequest.Emp.userId,
                ProcessId = leaveRequest.Process.Workflowactions.FirstOrDefault().Processid,
                Name = leaveRequest.Emp.Fname + " " + leaveRequest.Emp.Lname,
                LeaveType = leaveRequest.Leavetype,
                StartDate = leaveRequest.Startdate,
                EndDate = leaveRequest.Enddate,
                TotalDays = leaveRequest.Totaldays,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Process.Status,
                RequestDate = leaveRequest.Process.Requestdate,
                File = leaveRequest.File,
                FileUrl = $"{request.Scheme}://{request.Host}/files/{leaveRequest.File}",
                History = leaveRequest.Process.Workflowactions.Select(action => new
                {
                    ActionDate = action.Actiondate,
                    Action = action.Action,
                    Comments = action.Comments
                }).ToList()
            };
        }

        public async Task<object> GetAllLeaveRequestsPagingAsync(QueryObjectLeaveRequest query)
        {
            var leaveRequests = _leaveRequestRepository.GetAllLeaveRequestsAsync();
            var temp = leaveRequests.AsQueryable();

            // Keyword search
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                string keywordLower = query.Keyword.ToLower();
                temp = temp.Where(b => b.Requestid.ToString().ToLower().Contains(keywordLower) ||
                                       b.Leavetype.ToLower().Contains(keywordLower) ||
                                       b.Process.Status.ToLower().Contains(keywordLower) ||
                                       b.Process.Requestdate.ToString().ToLower().Contains(keywordLower) ||
                                       b.Emp.Fname.ToLower().Contains(keywordLower) ||
                                       b.Emp.Lname.ToLower().Contains(keywordLower));
            }

            // ID filter
            if (query.Id.HasValue)
            {
                temp = temp.Where(b => b.Requestid == query.Id.Value);
            }

            // Name filter
            if (!string.IsNullOrEmpty(query.Name))
            {
                string nameLower = query.Name.ToLower();
                temp = temp.Where(b => (b.Emp.Fname + " " + b.Emp.Lname).ToLower().Contains(nameLower));
            }

            // Leave Type filter
            if (!string.IsNullOrEmpty(query.LeaveType))
            {
                temp = temp.Where(b => b.Leavetype.ToLower().Contains(query.LeaveType.ToLower()));
            }

            // Current Status filter
            if (!string.IsNullOrEmpty(query.CurrentStatus))
            {
                temp = temp.Where(b => b.Process.Status.ToLower().Contains(query.CurrentStatus.ToLower()));
            }

            // Submission Date filter
            if (!string.IsNullOrEmpty(query.RequestDate))
            {
                if (DateTime.TryParse(query.RequestDate, out DateTime lastUpdatedDate))
                {
                    temp = temp.Where(e => e.Process.Requestdate.HasValue && e.Process.Requestdate.Value.Date == lastUpdatedDate.Date);
                }
            }

            var total = temp.Count();

            // Sorting logic
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "fname":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Emp.Fname)
                            : temp.OrderByDescending(s => s.Emp.Fname);
                        break;
                    case "lname":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Emp.Lname)
                            : temp.OrderByDescending(s => s.Emp.Lname);
                        break;
                    case "leavetype":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Leavetype)
                            : temp.OrderByDescending(s => s.Leavetype);
                        break;
                    case "requestdate":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Process.Requestdate)
                            : temp.OrderByDescending(s => s.Process.Requestdate);
                        break;
                    case "status":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Process.Status)
                            : temp.OrderByDescending(s => s.Process.Status);
                        break;
                    default:
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Requestid)
                            : temp.OrderByDescending(s => s.Requestid);
                        break;
                }
            }


            // Pagination logic
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var leaveRequestList = await temp.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            return new { total, data = leaveRequestList };
        }

    }
}
