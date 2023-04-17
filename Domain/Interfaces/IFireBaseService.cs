using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFireBaseService
    {
        Task<string> UploadStorage(Stream FileStream, string DestinationFolder, string FileName);
        Task<bool> DeleteStorage(string DestinationFolder, string FileName);
    }
}
