using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVirtualClinic
{
    abstract public class AbstractImageUploaderImplementation : IImageUploader
    {
        protected string _webServer;
        protected string _user;
        protected string _password;

        public void UploadImage(ICollection<DecoratedMediaFile> decoratedMediaFiles, string webServer, string user, string password)
        {
            System.Diagnostics.Debug.WriteLine("Test harness for Image Uploading...");
            _webServer = webServer;
            _user = user;
            _password = password;

            RunAsync(decoratedMediaFiles).Wait();
        }

        async Task RunAsync(ICollection<DecoratedMediaFile> decoratedMediaFiles)
        {
            await HttpPost("Submit/Edit", decoratedMediaFiles);
            return;
        }

        abstract protected Task HttpPost(string uri, ICollection<DecoratedMediaFile> decoratedMediaFiles);



    }
}
