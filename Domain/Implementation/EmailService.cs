using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;
using Data.DBContext;

namespace Domain.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IGenericRepository<Configuration> _repository;

        public EmailService(IGenericRepository<Configuration> repository)
        {
            _repository = repository;
        }
        public async Task<bool> SendEmail(string Email, string Subject, string Message)
        {
            try
            {
                IQueryable<Configuration> query = await _repository.Consult(c => c.Resource.Equals("Email_Service"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector:c=>c.Property, elementSelector:c=>c.Value);

                var credentials = new NetworkCredential(Config["email"], Config["password"]);

                var email = new MailMessage()
                {
                    From = new MailAddress(Config["email"], Config["alias"]),
                    Subject = Subject,
                    Body = Message,
                    IsBodyHtml = true
                };

                email.To.Add(new MailAddress(Email));

                var clientServer = new SmtpClient()
                {
                    Host = Config["host"],
                    Port = int.Parse(Config["port"]),
                    Credentials = credentials,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                clientServer.Send(email);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
