using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using XLabs.Platform.Services.Media;

namespace MyVirtualClinic.UWP
{
    class MediaPicker : IMediaPicker
    {
        public bool IsCameraAvailable
        {
            get
            {
                return true;
                throw new NotImplementedException();
            }
        }

        public bool IsPhotosSupported
        {
            get
            {
                return true;
                throw new NotImplementedException();
            }
        }

        public bool IsVideosSupported
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public EventHandler<MediaPickerErrorArgs> OnError
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public EventHandler<MediaPickerArgs> OnMediaSelected
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<MediaFile> SelectVideoAsync(VideoMediaStorageOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.VerySmallQvga;
            
StorageFile origPhoto = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);


            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder", CreationCollisionOption.OpenIfExists);

            StorageFile photo = await origPhoto.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
            await origPhoto.DeleteAsync();

            //MediaCapture mediaCapture = new MediaCapture();
            //var stream = new InMemoryRandomAccessStream();
            //await mediaCapture.CapturePhotoToStreamAsync(Windows.Media.MediaProperties.ImageEncodingProperties.CreateJpeg(), stream);


            if (photo!=null) {
                //using (Stream stream = await photo.OpenStreamForReadAsync())
                //{
                //    MemoryStream ms = new MemoryStream();
                //    await stream.CopyToAsync(ms);
                //    MediaFile mf = new MediaFile(photo.FolderRelativeId, () => { return ms; },null);
                //    return mf;
                //}
                  using (IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read)) {                 
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    
                    Stream ms= stream.AsStreamForRead();

                    //bitmapImage
                    //MediaFile mf = new MediaFile(photo.FolderRelativeId, () => { return stream.AsStream(); }, null);
                    //return mf;
                    //
                    //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    //SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    ////
                    //SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                    //               BitmapPixelFormat.Bgra8,
                    //               BitmapAlphaMode.Premultiplied);

                    //softwareBitmapBGR8.Fr

                    //
                    ////byte[] imageBytes = new byte[4 * decoder.PixelWidth * decoder.PixelHeight];
                    //byte[] imageBytes = new byte[];
                    //softwareBitmapBGR8.CopyToBuffer(imageBytes.AsBuffer());
                    ////  KnownFolders.PicturesLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByDate,0,20)
                    //
                    ////StorageFolder picturesFolder;
                    ////picturesFolder = KnownFolders.PicturesLibrary;
                    ////IReadOnlyList<StorageFile> sortedItems;                   
                    ////sortedItems = await picturesFolder.GetFilesAsync();
                    ////foreach (StorageFile file in sortedItems) { System.Diagnostics.Debug.WriteLine(file.Name + ", " + file.DateCreated); }
                    //
                    //
                    //
                    //
                    //MemoryStream ms = new MemoryStream( stream.AsStream().); //new MemoryStream( imageBytes );
                    //MemoryStream ms = stream.AsStreamForWrite();

                   
                //using (StreamReader sr = new StreamReader(stream.AsStream()))
                //{
                //    ms = sr.BaseStream;
                //}
                //ndows.Storage.KnownFolders.CameraRoll..CreateFolderQuery().

 //var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync
 //       (Windows.Storage.KnownLibraryId.Pictures);


                //await softwareBitmapBGR8.CopyToAsync(ms);
                // MediaFile mf = new MediaFile(photo.FolderRelativeId, () => { return stream; }, null);
                MediaFile mf = new MediaFile(photo.Path, () => { return ms; }, null);
                    photo =null;
                return mf;

                }
            }

            return null;
        }

        public Task<MediaFile> TakeVideoAsync(VideoMediaStorageOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
