// ***********************************************************************
// Assembly         : XLabs.Sample
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CameraPage.xaml.cs" company="XLabs Team">
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
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace MyVirtualClinic
    { 
    /// <summary>
    /// Class CameraPage.
    /// </summary>
    public partial class CameraView : ContentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPage"/> class.
        /// </summary>
        public CameraView()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Convenience function to allow mainpage access to images.
        /// Todo is there a better way to do this e.g. have the main page go to the model directly.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DecoratedMediaFile> GetPictures() {
            return (BindingContext as CameraViewModel).DecoratedMediaFiles;
        }

        public  void SetPictures(ObservableCollection<ImageSource> images)
        {
            (BindingContext as CameraViewModel).imageSources = images;
        }

        /// <summary>
        /// Todo - perhaps clearing the preview image should a be a function within the CameraViewModel...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGestureRecognizerPreviewTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Picture tapped...");
            var imageSender = (Image)sender;
            ((CameraViewModel)imageSender.BindingContext).ImagePreview = null;
            //imageSender.ClearValue(imageSender.Source.);
        }
    }
}
