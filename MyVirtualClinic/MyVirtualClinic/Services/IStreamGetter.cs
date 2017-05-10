using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

/// <summary>
/// Different os (iOs, Android, Windows) have different ways to find files from MediaFiles.
/// This interface allows us to get files as Stream or ImageSource.
/// </summary>
namespace MyVirtualClinic
{
    public interface IStreamGetter
    {
        ImageSource GetImageSource(MediaFile mediaFile);

        Stream GetStream(MediaFile mediaFile);
    }
}
