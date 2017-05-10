// ***********************************************************************
// Assembly         : XLabs.Sample
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CameraViewModel.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>

//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>

// ***********************************************************************
// 
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

using MyVirtualClinic.Services;
using System.Collections.ObjectModel;

//djkusing XLabs.Samples.Pages.Services;


//djknamespace XLabs.Samples.ViewModel
namespace MyVirtualClinic
{
    /// <summary>
    /// Class CameraViewModel.
    /// </summary>

    [ViewType(typeof(CameraView))]
    public class CameraViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        /// <summary>
        /// The _scheduler.
        /// </summary>
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// The picture chooser.
        /// </summary>
        private IMediaPicker _mediaPicker;

        /// <summary>
        /// The image source.
        /// </summary>
        private ImageSource _imageSource;

        /// <summary>
        /// The image currently selected to be shown big.
        /// </summary>
        private ImageSource _imagePreview;

        /// <summary>
        /// The video info.
        /// </summary>
        private string _videoInfo;

        /// <summary>
        /// The take picture command.
        /// </summary>
        private Command _takePictureCommand;
        
        private Command<object> _ExpandPictureCommand;

        private Command<object> _DeletePictureCommand;

        /// <summary>
        /// The select video command.
        /// </summary>
        private Command _selectVideoCommand;

        private string _status;

        ////private CancellationTokenSource cancelSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModel" /> class.
        /// </summary>
        public CameraViewModel()
        {
            Setup();

            DecoratedMediaFiles.CollectionChanged += ((s, e) => _takePictureCommand.ChangeCanExecute());

        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        //public ImageSource ImageSource
        //{
        //    get
        //    {
        //        return _imageSource;
        //    }
        //    set
        //    {
        //        SetProperty(ref _imageSource, value);
        //    }
        //}

        /// <summary>
        /// ImagePreview can be expanded on the page
        /// </summary>
        public ImageSource ImagePreview
        {
            get
            {
                return _imagePreview;
            }
            set
            {
                SetProperty(ref _imagePreview, value);
            }
        }

        /// <summary>
        /// Gets or sets the video info.
        /// </summary>
        /// <value>The video info.</value>
        public string VideoInfo
        {
            get
            {
                return _videoInfo;
            }
            set
            {
                SetProperty(ref _videoInfo, value);
            }
        }

        /// <summary>
        /// Gets the take picture command.
        /// </summary>
        /// <value>The take picture command.</value>
        public Command TakePictureCommand
        {
            get
            {
                return _takePictureCommand ?? (_takePictureCommand = new Command(
                                                                       async () => await TakePicture(),
                                                                       () => { return DecoratedMediaFiles.Count < 3; }));                
            }
        }
                
        /// <summary>
        /// Gets the select video command.
        /// </summary>
        /// <value>The select picture command.</value>
        public Command SelectVideoCommand
        {
            get
            {
                return _selectVideoCommand ?? (_selectVideoCommand = new Command(
                                                                       async () => await SelectVideo(),
                                                                       () => true));
            }
        }

        /// <summary>
        /// Gets the select picture command.
        /// </summary>
        /// <value>The select picture command.</value>
        //public Command SelectPictureCommand
        //{
        //    get
        //    {
        //        return _selectPictureCommand ?? (_selectPictureCommand = new Command(
        //                                                                   async () => await SelectPicture(),
        //                                                                   () => true));
        //    }
        //}

        public Command<object> ExpandPictureCommand {
            get {
                return _ExpandPictureCommand ?? (_ExpandPictureCommand = new Command<object>(
                                                                            async (mi) => await ExpandPicture(mi),
                                                                    (mi)=>true));
            }               
        }
                
        public Command<object> DeletePictureCommand
        { 
            get
            {
                return _DeletePictureCommand ?? (_DeletePictureCommand = new Command<object>(
                                                                            async (mi) => await DeletePicture(mi),
                                                                            (mi) => { return true;  }));
            }
        }

        private async Task DeletePicture(object mi) {
            Debug.WriteLine("Delete clicked");
            DecoratedMediaFiles.Remove((DecoratedMediaFile)mi);
        }
       
        private async Task ExpandPicture(object mi) {
            Debug.WriteLine("Expand clicked");
            ImagePreview = ((DecoratedMediaFile)mi).ImageSource;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get { return _status; }
            private set { SetProperty(ref _status, value); }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        private void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();
            ////RM: hack for working on windows phone? 
            _mediaPicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker ;
        }

        /// <summary>
        /// Takes the picture.
        /// </summary>
        /// <returns>Take Picture Task.</returns>
        private async Task<MediaFile> TakePicture()
        {
            Setup();
            // ImageSource = null;
            // https://forums.xamarin.com/discussion/42600/how-to-take-picture-from-camera-and-retrieve-it-using-xamarin-forms-for-cross-platform-app
            var options = new CameraMediaStorageOptions {
                DefaultCamera = CameraDevice.Front,
                MaxPixelDimension = 400 };

            // https://www.youtube.com/watch?v=LiHmSQCjxIA
            return await _mediaPicker.TakePhotoAsync(options).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Status = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    Status = "Canceled";
                }
                else
                {
                    var mediaFile = t.Result;
                    //mediaFile.Exif.Orientation= ExifLib.ExifOrientation.
                    // List template items on the view are bound to decoratedMediaFile instances.
                    decoratedMediaFiles.Add(new DecoratedMediaFile(mediaFile));
                    //imageHandler.MediaFile = mediaFile;

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        public ObservableCollection<ImageSource> imageSources = new ObservableCollection<ImageSource>();

        public ObservableCollection<ImageSource> ImageSources
        { 
            get
            {
                return imageSources;
            }
            set
            {
                SetProperty(ref imageSources, value);
            }
        }

        public ImageHandler imageHandler = new ImageHandler();
         
        /// <summary>
        /// Selects the picture.
        /// </summary>
        /// <returns>Select Picture Task.</returns>
        //private async Task SelectPicture()
        //{
        //    Setup();

        //    ImageSource = null;
        //    try
        //    {
        //        var mediaFile = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
        //        {
        //            DefaultCamera = CameraDevice.Front,
        //            MaxPixelDimension = 400
        //        });
        //        ImageSource = ImageSource.FromStream(() => mediaFile.Source);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Status = ex.Message;
        //    }
        //}
        

        /// <summary>
        /// Selects the video.
        /// </summary>
        /// <returns>Select Video Task.</returns>
        private async Task SelectVideo()
        {
            Setup();

            //TODO Localize
            VideoInfo = "Selecting video";

            try
            {
                var mediaFile = await _mediaPicker.SelectVideoAsync(new VideoMediaStorageOptions());

                //TODO Localize
                VideoInfo = mediaFile != null
                                ? string.Format("Your video size {0} MB", ConvertBytesToMegabytes(mediaFile.Source.Length))
                                : "No video was selected";
            }
            catch (System.Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    //TODO Localize
                    VideoInfo = "Selecting video canceled";
                }
                else
                {
                    VideoInfo = ex.Message;
                }
            }
        }

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private System.Collections.ObjectModel.ObservableCollection<DecoratedMediaFile> decoratedMediaFiles = new ObservableCollection<DecoratedMediaFile>();

        public ObservableCollection<DecoratedMediaFile> DecoratedMediaFiles
        {
            get
            {
                return decoratedMediaFiles;
            }
            set
            {
                SetProperty(ref decoratedMediaFiles, value);
            }
        }
    }
}
