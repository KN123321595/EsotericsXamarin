﻿using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace EsotericsXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenPage : ContentPage
    {
        //private FileResult photo;
        private readonly HttpClient client = new HttpClient();

        private MediaFile mediaFile;


        public ScreenPage()
        {
            InitializeComponent();

            
        }

        private async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {
                //photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                //{
                //    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                //});

                //var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                //var stream = await photo.OpenReadAsync();

                //var newStream = File.OpenWrite(newFile);

                //await stream.CopyToAsync(newStream);

                //image.Source = ImageSource.FromFile(photo.FullPath);

                await CrossMedia.Current.Initialize();

                mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg",
                });
                
                if (mediaFile == null)
                {
                    return;
                }

                image.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile.GetStream();
                });

            }

            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        private async void GetPhotoAsync(object sender, EventArgs e)
        {
            //try
            //{
            //    photo = await MediaPicker.PickPhotoAsync();

            //    image.Source = ImageSource.FromFile(photo.FullPath);
            //}

            //catch (Exception ex)
            //{
            //    await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            //}

            try
            {
                await CrossMedia.Current.Initialize();

                mediaFile = await CrossMedia.Current.PickPhotoAsync();

                if (mediaFile == null)
                    return;

                image.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile.GetStream();
                });
            }

            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        private async void enterBut_Clicked(object sender, EventArgs e)
        {
            //var authData = string.Format("{0}:{1}", "admin", "admin666");
            //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(mediaFile.GetStream()), "\"image\"", mediaFile.Path);

            
            HttpResponseMessage response = await client.PostAsync(ServerUrls.importImageUrl, content);
            
            string result = await response.Content.ReadAsStringAsync();

            

            

        }
    }
}