using MiniProject7.Domain.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject7.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        bool SendMail(MailData mailData);
    }
}
