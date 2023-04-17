using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using System.Security.Cryptography;


namespace Domain.Implementation
{
    public class UtilitiesService : IUtilitiesService
    {
        public string GeneratePassword()
        {
            string password = Guid.NewGuid().ToString("N").Substring(0,6);
            return password;
        }

        public string EncryptSha256(string text)
        {
            StringBuilder sb = new StringBuilder();

            using(SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach(byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();  
        }


    }
}
