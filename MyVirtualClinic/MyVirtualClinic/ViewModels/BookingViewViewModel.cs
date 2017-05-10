using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVirtualClinic
{
    class BookingViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string _mainText = "Main  text for BookingViewViewModel";

        public string MainText
        {
            get { return _mainText; }
            set { _mainText = value; }
        }
    }
}
