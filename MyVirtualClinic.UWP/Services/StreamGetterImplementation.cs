using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVirtualClinic;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.UWP.StreamGetterImplementation))]
namespace MyVirtualClinic.UWP
{
    class StreamGetterImplementation : IStreamGetter
    {
        public Stream GetStream(MediaFile mediaFile)
        {
            try
            {
                return new FileStream(mediaFile.Path, FileMode.Open);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("StreamGetterImplementation exceptio: " + ex.Message);
            }

            return null;
        }

        ImageSource IStreamGetter.GetImageSource(MediaFile mediaFile)
        {
            //return ImageSource.FromFile(mediaFile.Path);
            return ImageSource.FromStream(()=>new FileStream(mediaFile.Path, FileMode.Open));
        }
    }
}
