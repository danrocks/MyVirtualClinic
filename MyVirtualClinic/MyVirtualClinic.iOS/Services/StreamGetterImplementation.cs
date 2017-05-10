using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.iOS.StreamGetterImplementation))]
namespace MyVirtualClinic.iOS
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