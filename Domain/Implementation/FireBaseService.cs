using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using Entity;
using Data.Interfaces;

namespace Domain.Implementation
{
    public class FireBaseService : IFireBaseService
    {
        private readonly IGenericRepository<Configuration> _repository;

        public FireBaseService(IGenericRepository<Configuration> repository)
        {
            _repository = repository;
        }

        public async Task<string> UploadStorage(Stream FileStream, string DestinationFolder, string FileName)
        {
            string PicUrl = "";
            try
            {
                IQueryable<Configuration> query = await _repository.Consult(c => c.Resource.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Property, elementSelector: c => c.Value);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["FireBase_Storage"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["password"]);
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["path"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                    )
                    .Child(Config[DestinationFolder])
                    .Child(FileName)
                    .PutAsync(FileStream, cancellation.Token);

                PicUrl = await task;
            }
            catch
            {
                PicUrl = "";
            }

            return PicUrl;
        }

        public async Task<bool> DeleteStorage(string DestinationFolder, string FileName)
        {
            try
            {
                IQueryable<Configuration> query = await _repository.Consult(c => c.Resource.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Property, elementSelector: c => c.Value);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["FireBase_Storage"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["password"]);
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["path"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                    )
                    .Child(Config[DestinationFolder])
                    .Child(FileName)
                    .DeleteAsync();

                await task;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
