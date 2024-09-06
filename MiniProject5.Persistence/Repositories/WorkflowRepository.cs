using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.Interfaces.IRepositories;
using MiniProject6.Domain.Models;
using MiniProject6.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Persistence.Repositories
{
    public class WorkflowRepository:IWorkflowRepository
    {
        private readonly HrisContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public WorkflowRepository(HrisContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request)
        {
            request.Startdate = DateTime.Now;
            request.Enddate = request.Startdate?.AddDays(7);

            var process = new Process
            {
                Workflowid = 1,
                Requestdate = DateTime.Now,
                Status = "Pending",
                Currentstepid = 2,
                Requesttype = "Leave Request",
                Requesterid = request.Process.Requesterid
            };

            //workflowaction

            await _context.Processes.AddAsync(process);
            request.Process = process;

            await _context.Leaverequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<Workflowaction> AddActionAsync(Workflowaction workflowAction)
        {
            workflowAction.Actiondate = DateTime.Now;
            await _context.Workflowactions.AddAsync(workflowAction);
            await _context.SaveChangesAsync();
            return workflowAction;
        }
        
        public async Task ApproveOrRejectLeaveRequestBySupervisorAsync(int processId, Process Process)
        {
            var existingProcess = await _context.Processes.FirstOrDefaultAsync(cek=>cek.Processid == processId);
            
            if (existingProcess != null)
            {
                existingProcess.Status = Process.Status;
                if(Process.Status == "Approve")
                {
                    existingProcess.Currentstepid = 3;
                }

                if (Process.Status == "Reject")
                {
                    existingProcess.Currentstepid = 5;
                }
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task ApproveOrRejectLeaveRequestByHRAsync(int processId, Process Process)
        {
            var existingProcess = await _context.Processes.FirstOrDefaultAsync(cek=>cek.Processid == processId);
            
            if (existingProcess != null)
            {
                existingProcess.Status = Process.Status;
                if(Process.Status == "Approve")
                {
                    existingProcess.Currentstepid = 4;
                }

                if (Process.Status == "Reject")
                {
                    existingProcess.Currentstepid = 5;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
