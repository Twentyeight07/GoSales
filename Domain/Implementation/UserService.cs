using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;

namespace Domain.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IFireBaseService _fireBaseService;
        private readonly IUtilitiesService _UtilitiesService;
        private readonly IEmailService _emailService;

        public UserService(
            IGenericRepository<User> repository, IFireBaseService fireBaseService, IUtilitiesService UtilitiesService, IEmailService emailService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
            _UtilitiesService = UtilitiesService;
            _emailService = emailService;
        }

        public async Task<List<User>> List()
        {
            IQueryable<User> query = await _repository.Consult();
            return query.Include(r => r.RoleIdNavigation).ToList();
        }

        public async Task<User> Create(User entity, Stream userPic = null, string picName = "", string EmailTemplateUrl = "")
        {
            User userExist= await _repository.Get(u => u.Email == entity.Email);

            if (userExist != null)
                throw new TaskCanceledException("El correo ya existe.");

            try
            {
                string generatedPass = _UtilitiesService.GeneratePassword();
                entity.Password = _UtilitiesService.EncryptSha256(generatedPass);
                entity.PicName = picName;

                if(userPic != null)
                {
                    string picUrl = await _fireBaseService.UploadStorage(userPic, "user_folder", picName);
                    entity.PicUrl = picUrl;
                }

                User created_user = await _repository.Create(entity);

                if(created_user.UserId == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario.");


                if(EmailTemplateUrl != "")
                {
                    EmailTemplateUrl = EmailTemplateUrl.Replace("[email]", created_user.Email).Replace("[password]", generatedPass);

                    string htmlEmail = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EmailTemplateUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader streamReader = null;

                            if(response.CharacterSet == null)
                            {
                                streamReader = new StreamReader(dataStream);
                            }
                            else
                            {
                                streamReader = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            htmlEmail = streamReader.ReadToEnd();
                            response.Close();
                            streamReader.Close();
                        }
                    }

                    if(htmlEmail != "")
                        await _emailService.SendEmail(created_user.Email, "Cuenta creada", htmlEmail);
                    
                }

                IQueryable<User> query = await _repository.Consult(u => u.UserId == created_user.UserId);
                created_user = query.Include(r => r.RoleIdNavigation).First();

                return created_user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> Edit(User entity, Stream UserPic = null, string PicName = "")
        {
            User userExist = await _repository.Get(u => u.Email == entity.Email && u.UserId != entity.UserId);

            if (userExist != null)
                throw new TaskCanceledException("El correo ya existe.");
            

            try
            {
                IQueryable<User> userQuery = await _repository.Consult(u => u.UserId == entity.UserId);

                User edit_user = userQuery.First();
                edit_user.Name = entity.Name;
                edit_user.Email = entity.Email;
                edit_user.Phone = entity.Phone;
                edit_user.RoleId = entity.RoleId;
                edit_user.IsActive = entity.IsActive;

                if (edit_user.PicName == "")
                    edit_user.PicName = PicName;

                if(edit_user.PicName != null)
                {
                    string PicUrl = await _fireBaseService.UploadStorage(UserPic, "user_folder", edit_user.PicName);
                    edit_user.PicUrl = PicUrl;
                }

                bool response = await _repository.Edit(edit_user);

                if (!response)
                    throw new TaskCanceledException("No se pudo editar el usuario.");

                User edited_user = userQuery.Include(r => r.RoleIdNavigation).First();
                return edited_user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int UserId)
        {
            try
            {
                User userFound = await _repository.Get(u => u.UserId == UserId);

                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe.");

                string PicName = userFound.PicName;
                bool res = await _repository.Delete(userFound);

                if (res)
                    await _fireBaseService.DeleteStorage("user_folder", PicName);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<User> GetByCredentials(string email, string password)
        {
            string encryptedPass = _UtilitiesService.EncryptSha256(password);

            User userFound = await _repository.Get(u => u.Email.Equals(email) && u.Password.Equals(encryptedPass));

            return userFound;
        }

        public async Task<User> GetById(int UserId)
        {
            IQueryable<User> query = await _repository.Consult(u => u.UserId == UserId);

            User res = query.Include(u => u.RoleIdNavigation).FirstOrDefault();

            return res;
        }

        public async Task<bool> SaveProfile(User entity)
        {
            try
            {
                User userFound = await _repository.Get(u => u.UserId == entity.UserId);

                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe.");

                userFound.Email = entity.Email;
                userFound.Phone = entity.Phone;

                bool res = await _repository.Edit(userFound);

                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(int UserId, string ActualPassword, string NewPassword)
        {
            try
            {
                User userFound = await _repository.Get(u => u.UserId == UserId);

                if(userFound == null)
                    throw new TaskCanceledException("El usuario no existe.");

                if(userFound.Password != _UtilitiesService.EncryptSha256(ActualPassword))
                    throw new TaskCanceledException("Contraseña incorrecta.");

                userFound.Password = _UtilitiesService.EncryptSha256(NewPassword);

                bool res = await _repository.Edit(userFound);

                return res;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> RestorePassword(string email, string EmailTemplateUrl)
        {
            try
            {
                User userFound = await _repository.Get(u => u.Email == email);

                if (userFound == null)
                    throw new TaskCanceledException("No se encontró un usuario asociado con el correo ingresado.");

                string generatedPass = _UtilitiesService.GeneratePassword();
                userFound.Password = _UtilitiesService.EncryptSha256(generatedPass);

                EmailTemplateUrl = EmailTemplateUrl.Replace("[password]", generatedPass);

                string htmlEmail = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EmailTemplateUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader streamReader = null;

                        if (response.CharacterSet == null)
                        {
                            streamReader = new StreamReader(dataStream);
                        }
                        else
                        {
                            streamReader = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        htmlEmail = streamReader.ReadToEnd();
                        response.Close();
                        streamReader.Close();
                    }
                }

                bool emailSent = false;

                if (htmlEmail != "")
                    emailSent = await _emailService.SendEmail(email, "Contraseña Reestablecida", htmlEmail);

                if (!emailSent)
                    throw new TaskCanceledException("Actualmente estamos presentando problemas para llevar a cabo su solicitud, favor intentar nuevamente más tarde.");

                bool res = await _repository.Edit(userFound);
                return res;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
