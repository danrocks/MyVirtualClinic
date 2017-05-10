using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Forms.Mvvm;

using MyVirtualClinic;

namespace MyVirtualClinic
{
    [ViewType(typeof(PersonView))]
    class PersonViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        private Person person = new Person(true);
        
        /// <summary>
        /// Date of Birth
        /// </summary>
        private DateTime _Dob = DateTime.Now;

        public string FirstName
        {
            get { return person.FirstName; }
            set
            {
                if (person.FirstName == value)
                    return;

                //SetProperty(ref person.FirstName, value);
                person.FirstName = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("FirstName"));
            }
        }


        public string LastName { get { return person.LastName; }
                        set {
                if (person.LastName == value)
                    return;

                person.LastName = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("LastName"));
            }
        }

        //protected void OnPropertyChanged(string propertyName) {
        //    PropertyChanged
        //}

        public DateTime Dob { get { return person.Dob; }
            set {
                if (person.Dob == value)
                    return;
                person.Dob = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Dob"));
            }
        }

        public Person Person
        {
            get
            {
                return person;
            }           
        }
    }
}
