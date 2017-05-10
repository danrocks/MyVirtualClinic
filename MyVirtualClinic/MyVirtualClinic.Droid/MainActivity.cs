using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


using XLabs.Ioc;
using XLabs.Forms;
using XLabs.Platform;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;
namespace MyVirtualClinic.Droid
{
    //https://github.com/XLabs/Xamarin-Forms-Labs
    //public class MainActivity : XFormsApplicationDroid
    [Activity(Label = "MyVirtualClinic", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // djk added as per http://www.codenutz.com/getting-started-xamarin-forms-labs-xaml-mvvm-ioc/
            // http://www.matrixguide.ch/Datenablage/diverses/How_to_Install_and_Setup_XLabs.pdf
            if (!Resolver.IsSet)
            {
                var container = new SimpleContainer();
                container.Register<IDevice> (t => AndroidDevice.CurrentDevice);
                container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
                container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
                Resolver.SetResolver(container.GetResolver()); // Resolving the services  // End new Xlabs
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

