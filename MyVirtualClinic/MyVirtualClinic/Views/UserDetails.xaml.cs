using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;

namespace MyVirtualClinic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetails : ContentPage
    {
        public UserDetails()
        {
            InitializeComponent();
            //BindingContext = new UserDetailsViewModel();


            // https://github.com/jamesmontemagno/ConnectivityPlugin/blob/master/samples/ConnectivitySample/ConnectivitySample/App.cs
            //https://www.youtube.com/watch?v=7RWCuo4VPu4
            //check active internet connection
            NetworkState.Text = CrossConnectivity.Current.IsConnected ? "Internet Connected" : "No active internet Connection";

            //Handle connection changes
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("CrossConnectivity.Current.ConnectivityChanged");
                this.DisplayAlert("Connectivity changed"
                    , "IsConnected: " + CrossConnectivity.Current.IsConnected
                        + "  Args:" + args.IsConnected.ToString(), "OK");
                    
                NetworkState.Text = CrossConnectivity.Current.IsConnected ? "Internet Connected" : "No active internet Connection";
            };

            //Handle connectivity type changes
            CrossConnectivity.Current.ConnectivityTypeChanged += (sender, args) =>
                  {
                      System.Diagnostics.Debug.WriteLine("CrossConnectivity.Current.ConnectivityTypeChanged");
                      this.DisplayAlert("Connectivity Type Changed"
                          , "Types: " + args.ConnectionTypes.FirstOrDefault()
                          , "OK");

                      NetworkState.Text = CrossConnectivity.Current.IsConnected ? "Internet Connected" : "No active internet Connection";

                  };

        }

    }    
}
