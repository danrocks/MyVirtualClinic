using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

namespace MyVirtualClinic.Services
{
    /// <summary>
    /// This class provides tools to convert an image to a stream
    /// </summary>
    //https://forums.xamarin.com/discussion/46808/imagesoue-to-stream
    public class ImageHandler : BindableObject
    {
        private byte[] ImageRaw { get; set; }

        public MediaFile MediaFile;       

        /// <summary>
        /// Get a MemoryStream from a MediaFile
        /// </summary>
        public Stream ImageStream
        {
            get
            {
                Stream stream = DependencyService.Get<IStreamGetter>().GetStream(MediaFile);
                if (stream != null)
                {
                    ImageRaw = new byte[stream.Length];
                    stream.Read(ImageRaw, 0, ImageRaw.Length);
                    
                    SetValue(ImageSourceProperty, ImageSource.FromStream(() => new MemoryStream(ImageRaw)));
                }
                return ImageRaw != null ? new MemoryStream(ImageRaw) : null;
            }
            //set
            //{
            //    if (value != null)
            //    {
            //        ImageRaw = new byte[value.Length];
            //        value.Read(ImageRaw, 0, ImageRaw.Length);

            //        SetValue(ImageSourceProperty, ImageSource.FromStream(() => new MemoryStream(ImageRaw)));
            //    }
            //}
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create<ImageHandler, ImageSource>(p => p.ImageSource, null);

        public ImageSource ImageSource
        {
            get
            {
                return ImageStream != null
                    ? ImageSource.FromStream(() => ImageStream)
                    : (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                ImageRaw = null;
                SetValue(ImageSourceProperty, value);
            }
        }
    }
}
