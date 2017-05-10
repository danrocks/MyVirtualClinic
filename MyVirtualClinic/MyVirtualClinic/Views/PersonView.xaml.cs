using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyVirtualClinic { 
    public partial class PersonView : ContentView
    {
        public PersonView()
        {
            InitializeComponent();
            DobPicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);
            DobPicker.SetValue(DatePicker.MinimumDateProperty, DateTime.Now.AddYears(-120));
        }

        /// <summary>
        /// Allow access to Model (a Person) via method on the View that access the viewmodel.
        /// </summary>
        /// <returns></returns>
        public Person GetPerson() {
            return (BindingContext as PersonViewModel).Person;
        }
    }
}
