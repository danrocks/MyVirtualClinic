using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace MyVirtualClinic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Padding = new Thickness(5,Device.OnPlatform(25,0,0),5,5);
            //PageImageSources = camView.GetPictures();

             ((UploadViewModel)uploadView.BindingContext).decoratedMediaFiles= camView.GetPictures();
             ((UploadViewModel)uploadView.BindingContext).Person = personView.GetPerson();

        }

        ObservableCollection<ImageSource> PageImageSources = new ObservableCollection<ImageSource>();

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserDetails());
        }
    }
}
