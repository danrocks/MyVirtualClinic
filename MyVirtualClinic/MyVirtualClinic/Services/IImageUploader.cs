using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// iOs & Android use System.Net.HttpCLint to post images, but UWP uses a different approach.
/// </summary>
namespace MyVirtualClinic { 
    public interface IImageUploader
    {
         void UploadImage(ICollection<DecoratedMediaFile> decoratedMediaFiles, string webServer, string user, string password);
    }
}
