using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.AttachmentService
{
    public interface IAttachmentService
    {
        string? Upload(string fileName, IFormFile file); 

        bool Delete(string fileName, string folderName);
    }
}
