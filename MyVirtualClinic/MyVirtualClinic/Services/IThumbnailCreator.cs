using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyVirtualClinic
{
    /// <summary>
    /// Class takes a an Image soruce and returns a thumbnail version
    /// https://blog.verslu.is/xamarin/but-without-my-voice-how-can-i-using-dependencyservice-to-implement-text-to-speech/
    /// </summary>
    public interface IThumbnailCreator
    {
          ImageSource Create(ImageSource imageSource, float width, float height, int quality);
    }
}
