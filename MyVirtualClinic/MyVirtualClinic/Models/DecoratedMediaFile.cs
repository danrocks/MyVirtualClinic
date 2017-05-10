using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

namespace MyVirtualClinic
{
    /// <summary>
    /// An instance of the (Xlabs.)MediaFile decorated with methods to convert 
    /// the media file to an ImageSource or a Stream.
    /// </summary>
    public class DecoratedMediaFile
    {
        private MediaFile MediaFile;

        public DecoratedMediaFile(MediaFile mediaFile)
        {
            MediaFile = mediaFile;
        }

        public ImageSource GetImageSource()
        {
            return DependencyService.Get<IStreamGetter>().GetImageSource(MediaFile);
        }

        private ImageSource _imageSrc = null;

        public ImageSource ImageSource
        {
            get
            {
                if (_imageSrc == null)
                {
                    return _imageSrc = DependencyService.Get<IStreamGetter>().GetImageSource(MediaFile);
                }
                return _imageSrc;
            }
        }

        private byte[] ImageRaw { get; set; }

        public Stream ImageStream
        {
            get
            {
                using (Stream stream = DependencyService.Get<IStreamGetter>().GetStream(MediaFile)) { 
                    if (stream != null)
                    {
                        ImageRaw = new byte[stream.Length];
                        stream.Read(ImageRaw, 0, ImageRaw.Length);

                        // SetValue(ImageSourceProperty, ImageSource.FromStream(() => new MemoryStream(ImageRaw)));
                    }
                return ImageRaw != null ? new MemoryStream(ImageRaw) : null;
                }
            }
        }

        public String Base64String {
            get {
                using (MemoryStream ms = new MemoryStream())
                {
                    ImageStream.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    // convert byte[] to Base64 String
                    return System.Convert.ToBase64String(imageBytes);
                }
            }
        }
    }
}