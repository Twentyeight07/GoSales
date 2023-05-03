using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> List();
        Task<User> Create(User entity, Stream UserPic = null, string PicName = "", string UrlEmailTemplate = "");
        Task<User> Edit(User entity, Stream UserPic = null, string PicName = "");
        Task<bool> Delete(int UserId);
        Task<User> GetByCredentials(string email, string password);
        Task<User> GetById(int UserId);
        Task<List<User>> GetByRole(int userRole);
        Task<bool> SaveProfile(User entity);
        Task<bool> ChangePassword(int UserId, string ActualPassword, string NewPassword);
        Task<bool> RestorePassword(string email, string UrlEmailTemplate);
    }
}
