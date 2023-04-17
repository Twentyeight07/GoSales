using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string Email, string Subject, string Message);
    }
}
