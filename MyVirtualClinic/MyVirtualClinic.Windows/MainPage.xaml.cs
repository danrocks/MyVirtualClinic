using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// New Xlabs 
using XLabs.Ioc;
using XLabs.Forms;
using XLabs.Platform;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;
// End new Xlabs 

namespace MyVirtualClinic.Windows
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            //// djk added as per http://www.codenutz.com/getting-started-xamarin-forms-labs-xaml-mvvm-ioc/
            //// http://www.matrixguide.ch/Datenablage/diverses/How_to_Install_and_Setup_XLabs.pdf
            //if (!Resolver.IsSet)
            //{
            //    var container = new SimpleContainer();
            //    container.Register<IDevice>(t => WindowsPhoneDevice.CurrentDevice);
            //    container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            //    container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
            //    Resolver.SetResolver(container.GetResolver()); // Resolving the services  // End new Xlabs
            //}

            LoadApplication(new MyVirtualClinic.App());
        }
    }
}
