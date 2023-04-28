using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> List();
    }
}
