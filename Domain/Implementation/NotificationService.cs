using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;

namespace Domain.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _repository;

        public NotificationService(IGenericRepository<Notification> repository)
        {
            _repository = repository;
        }

        public async Task<List<Notification>> List()
        {
            IQueryable<Notification> query = await _repository.Consult();
            return query.OrderByDescending(d => d.CreatedAt).Take(5).ToList();
        }
    }
}
