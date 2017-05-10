
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Platform.Services.Media;

namespace MyVirtualClinic
{

    [ViewType(typeof(UploadView))]
    public class UploadViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        /// <summary>
        /// Upload picture command
        /// </summary>
        private Command _uploadClinicRecordCommand;

        /// <summary>
        /// A reference to the CameraViewModel's list of pictures.
        /// Set by the page...
        /// </summary>
        private ObservableCollection<DecoratedMediaFile> _decoratedMediaFiles;

        public ObservableCollection<DecoratedMediaFile> decoratedMediaFiles
        {
            set{
                _decoratedMediaFiles = value;
                _decoratedMediaFiles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChangedHandler);                   
            }
        }

        private Person _person=null;
        public Person Person
        {
            set {
                _person = value;
            }
        }
        
        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e) {
            _uploadClinicRecordCommand.ChangeCanExecute();
        }

        public Command UploadClinicRecordCommand
        {
            get
            {
                return _uploadClinicRecordCommand ?? (_uploadClinicRecordCommand = new Command(
                                                       execute: async () => await UploadVirtualConsultInfo(),
                                                       canExecute: () => { return _decoratedMediaFiles == null?false: _decoratedMediaFiles.Count > 0; }));
            }
        }

        private async Task<MediaFile> UploadVirtualConsultInfo()
        {
            System.Diagnostics.Debug.WriteLine("Upload picture");

            User user = new User(true);


            const string webServer = @"https://myvirtualclinic.net/";
            //const string webServer = @"https://localhost:44379/";

            await Task.Factory.StartNew(() => DependencyService.Get<IImageUploader>().UploadImage(_decoratedMediaFiles, webServer, user.Email, user.Password));

            //return new MediaFile();
            return null;
        }

        string b64;
        private void Setb64(Stream s)
        {
            b64 = ConvertStream(s);
            return;
        }

        private string ConvertStream(Stream s)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();

                // convert byte[] to Base64 String
                return System.Convert.ToBase64String(imageBytes);
            }
        }
    }
}
