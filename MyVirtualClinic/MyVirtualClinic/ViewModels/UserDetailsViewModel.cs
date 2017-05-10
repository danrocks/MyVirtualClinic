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
using XLabs.Forms.Mvvm;

namespace MyVirtualClinic {
    [ViewType(typeof(UserDetails))]
    class UserDetailsViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        private User _user = new User(true);

        public string Email
        {
            get {
                return _user.Email; }
            set
            {
                if (_user.Email == value)
                    return;

                _user.Email = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Email"));
            }
        }

        public string Password
        {
            get {
                return _user.Password; }
            set
            {
                if (_user.Password == value)
                    return;

                _user.Password = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Password"));
            }
        }

        //public UserDetailsViewModel()
        //{
        //    IncreaseCountCommand = new Command(IncreaseCount);
        //}

        //int count;

        //string countDisplay = "You clicked 0 times.";
        //public string CountDisplay
        //{
        //    get { return countDisplay; }
        //    set { countDisplay = value; OnPropertyChanged(); }
        //}

        //public ICommand IncreaseCountCommand { get; }

        //void IncreaseCount() =>
        //    CountDisplay = $"You clicked {++count} times";


        //public event PropertyChangedEventHandler PropertyChanged;
        //void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
    }