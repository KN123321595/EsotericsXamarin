using EsotericsXamarin.pages;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        public static string fileName;


        public ScreenPage()
        {
            InitializeComponent();

            
        }

        public static ImportImageOriginal original;
        //кнопка выбрать фото
        private async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {

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
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        //кнопка сделать фото
        private async void GetPhotoAsync(object sender, EventArgs e)
        {


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
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        //кнопка complete, загружаем фото на сервер
        private async void enterBut_Clicked(object sender, EventArgs e)
        {

            try
            {

                //var stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folderPath, "MyImage.jpg")));


                if (mediaFile == null)
                    return;

                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                int numberPhoto = 1;

                //File.WriteAllText(Path.Combine(folderPath, "test.txt"), "gergrhth");

                IEnumerable<string> list = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));

                foreach (string str in list)
                {
                    if (str.Contains("jpg"))
                    {
                        numberPhoto++;
                    }
                }               

                StateOfActivityFrame();

                var memoryStream = new MemoryStream();

                await mediaFile.GetStream().CopyToAsync(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                fileName = "MyImage" + numberPhoto + ".jpg";

                File.WriteAllBytes(Path.Combine(folderPath, fileName), bytes);



                var content = new MultipartFormDataContent();

                content.Add(new StreamContent(mediaFile.GetStream()), "\"image\"", mediaFile.Path);


                HttpResponseMessage response = await client.PostAsync(ServerUrls.importImageUrl, content);

                string result = await response.Content.ReadAsStringAsync();

                ImportImage importImage = JsonConvert.DeserializeObject<ImportImage>(result);

                response = await client.GetAsync(ServerUrls.importImageOriginalUrl + importImage.most_similar_to);

                result = await response.Content.ReadAsStringAsync();

                original = JsonConvert.DeserializeObject<ImportImageOriginal>(result);



                await Navigation.PushAsync(new ResultPage());
            }

            catch(Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");

            }

            finally
            {
                StateOfActivityFrame();
            }

          

        }

        //управление состоянием индикатора загрузки
        private void StateOfActivityFrame()
        {
            if (activityFrame.IsVisible)
            {
                activityFrame.IsVisible = false;
                return;
            }

            else
            {
                activityFrame.IsVisible = true;
                return;
            }
        }
    }
}