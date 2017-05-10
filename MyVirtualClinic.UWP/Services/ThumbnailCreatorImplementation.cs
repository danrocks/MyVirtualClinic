using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVirtualClinic;
using Xamarin.Forms;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(MyVirtualClinic.UWP.ThumbnailCreatorImplementation))]
namespace MyVirtualClinic.UWP
{    
    class ThumbnailCreatorImplementation : IThumbnailCreator
    {
        ImageSource IThumbnailCreator.Create(ImageSource imageSource, float width, float height, int quality)
        {
  
            throw new NotImplementedException();
        }
    }
}
