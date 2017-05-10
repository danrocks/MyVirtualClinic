using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MyVirtualClinic;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.WinPhone.StreamGetterImplementation))]
namespace MyVirtualClinic.WinPhone
{
    class StreamGetterImplementation : IStreamGetter
    {      
            public Stream GetStream(MediaFile mediaFile)
            {
                return mediaFile.Source;
            }

            public ImageSource GetImageSource(MediaFile mediaFile)
            {
                return ImageSource.FromStream(() => mediaFile.Source);
            }
        }
}
