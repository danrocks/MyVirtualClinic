using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using System.IO;


[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.Droid.StreamGetterImplementation))]
namespace MyVirtualClinic.Droid
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