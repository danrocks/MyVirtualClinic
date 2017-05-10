using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyVirtualClinic
{
    public class Person
    {
        private string _FirstName = "f";
        private string _Lastname = "l";

        /// <summary>
        /// Date of Birth
        /// </summary>
        private DateTime _Dob = DateTime.Now;

        public Person() : this(false) {

        }

        /// <summary>
        /// UseCurrentProperties indicates whether an attempt should be made to instantiate 
        /// the new Person based on details stored in the Application.Current.Properties dictionary.
        /// This should be false when using Newtonsoft to deserialise a Json verson of the user
        /// </summary>
        /// <param name="UseCurrentProperties"></param>
        public Person(bool UseCurrentProperties) {
            if (UseCurrentProperties) {
                Initialise();
            }           
        }


        /// <summary>
        /// If applicatrion was previously closed, there may be stored peron data that we would like to populate the screen with when app
        /// restarts.
        /// </summary>
        private void Initialise() {
            string personJson = GetDictionaryEntry(Application.Current.Properties, "personJson", "");
                        
            if (!string.IsNullOrEmpty(personJson))
            {
                Person person = JsonConvert.DeserializeObject<Person>(personJson);
                _FirstName = person.FirstName;
                _Lastname = person.LastName;
                _Dob = person.Dob;                
            }
        }

        private T GetDictionaryEntry<T>(IDictionary<string, object> dict, string key, T DefaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return (T)dict[key];
            }

            return DefaultValue;
        }

        public string FirstName { get {return _FirstName; }
            set {
                _FirstName = value;
                Save();
            }
        }

        public string LastName { get { return _Lastname; } set {
                _Lastname = value;
                   Save();
            } }

        public DateTime Dob { get { return _Dob; }
            set { _Dob = value;
                Save();
            } }

        private void Save()
        {
            Application.Current.Properties["personJson"] = JsonConvert.SerializeObject(this);
            Application.Current.SavePropertiesAsync();
        }
    }
}
