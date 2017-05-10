using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyVirtualClinic
{
    class User
    {
        private string _Email="";
        private string _Password="";

        public User():this(false){
        }

        public bool IsValid() {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                return false;

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="UseCurrentProperties">
        /// UseCurrentProperties indicates whether an attempt should be made to 
        /// initialise User based on details in Application.Current.Properties dictionary.
        /// This should be false when using Newtonsoft to deserialise a Json verson of the user
        /// </param>
        public User(bool UseCurrentProperties)
        {
            if (UseCurrentProperties)
            {
                Initialise();
            }
        }

        private void Initialise()
        {
            string userJson = GetDictionaryEntry(Application.Current.Properties, "userJson", "");

            if (!string.IsNullOrEmpty(userJson))
            {
                User user = JsonConvert.DeserializeObject<User>(userJson);
                _Email = user.Email;
                _Password = user.Password;                
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

        public string Email {
            get {
                return _Email; }
            set {
                _Email = value;
                System.Diagnostics.Debug.WriteLine("Set userJson (Email:{0}) ", value);
                Save();
            } }

        public string Password {
            get {
                return _Password; }
            set {
                _Password = value;
                Save();    
            } }

        /// <summary>
        /// Some people say SavePropertiesAsync shouldn;t be required...but automatic saves to persistant storage
        /// only occur after an undefined time, so...
        /// </summary>
        private void Save() {
            Application.Current.Properties["userJson"] = JsonConvert.SerializeObject(this);
            Application.Current.SavePropertiesAsync();
        }
    }
}
