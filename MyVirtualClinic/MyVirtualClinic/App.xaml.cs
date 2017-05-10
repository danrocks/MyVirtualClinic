using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XLabs.Platform.Device;

using XLabs.Platform.Services;
using XLabs.Ioc;

using System.Diagnostics;

using XLabs.Platform.Services.Media;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace MyVirtualClinic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
       
            MainPage =new NavigationPage( new MyVirtualClinic.MainPage());
            
            //MainPage.FindByName<PersonView>("personView");

            //string serialisedImages = GetDictionaryEntry(Current.Properties, "images", "");
            //if (!string.IsNullOrEmpty(serialisedImages)) {
            //    ObservableCollection<ImageSource> images = JsonConvert.DeserializeObject<ObservableCollection<ImageSource>>(serialisedImages);
            //    CameraView camView = MainPage.FindByName<CameraView>("camView");
            //    camView.SetPictures( images );
            //}
            Debug.WriteLine("App initialised");
        }

        private T GetDictionaryEntry<T>(IDictionary<string, object> dict, string key, T DefaultValue) {
            if (dict.ContainsKey(key)) {
                return (T)dict[key];
            }

            return DefaultValue;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Debug.WriteLine("App starting");            
        }

        protected override void OnSleep()
        {
           // Handle when your app sleeps
           //CameraView camView = MainPage.FindByName<CameraView>("camView");
           // if (camView!=null){
           //     ObservableCollection<DecoratedMediaFile> images = camView.GetPictures();
           //     Application.Current.Properties.Add("images", JsonConvert.SerializeObject(images));
           // }
            
           Debug.WriteLine("App sleeping");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("App resuming");
        }
    }
}
