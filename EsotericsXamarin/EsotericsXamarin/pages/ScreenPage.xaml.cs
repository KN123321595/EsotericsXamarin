using EsotericsXamarin.pages;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        private bool exitTimer = false;


        public static string fileName;
        public static double percent;

        public static ImportImageOriginal original;


        public ScreenPage()
        {
            InitializeComponent();

            TakePhotoBtn.Text = AppResources.TakePhotoText;
            getPhotoBtn.Text = AppResources.GetPhotoText;
            enterBut.Text = AppResources.CompleteText;
            againLabel.Text = AppResources.AgainText;
            
        }

        
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

                

                var content = new MultipartFormDataContent();

                content.Add(new StreamContent(mediaFile.GetStream()), "\"image\"", mediaFile.Path);


                HttpResponseMessage response = await client.PostAsync(ServerUrls.importImageUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    await DisplayAlert(null, $"{AppResources.ErrorServerText}", "OK");
                    StateOfActivityFrame();
                    return;
                }


                var memoryStream = new MemoryStream();

                await mediaFile.GetStream().CopyToAsync(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                fileName = "MyImage" + numberPhoto + ".jpg";

                File.WriteAllBytes(Path.Combine(folderPath, fileName), bytes);



                string result = await response.Content.ReadAsStringAsync();


                ImportImage importImage = JsonConvert.DeserializeObject<ImportImage>(result);

                percent = importImage.percent;


                response = await client.GetAsync(ServerUrls.importImageOriginalUrl + importImage.most_similar_to);

                result = await response.Content.ReadAsStringAsync();

                original = JsonConvert.DeserializeObject<ImportImageOriginal>(result);


                StateOfActivityFrame();

                await Navigation.PushAsync(new ResultPage());
            }

            catch(Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
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

        //переопределение метода нажатия кнопки назад
        protected override bool OnBackButtonPressed()
        {

            AnimationPopup();

            return true;

        }

        //управление анимацией всплывающего окна при нажатии кнопки назад
        private async void AnimationPopup()
        {


            if (!popupLayout.IsVisible)
            {
                popupLayout.IsVisible = !popupLayout.IsVisible;
                //this.popuplayout.AnchorX = 1;
                //this.popuplayout.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => popupLayout.Scale = f,
                    0,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => popupLayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(popupLayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(popupLayout, "popupFadeAnimation", 250);

                //запускаем таймер на 2 секунды для подтверждения выхода из приложения
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    exitTimer = false;

                    AnimationPopup();

                    return false;
                });

                exitTimer = true;
            }
            else
            {
                //если нажимаем на кнопку назад повторно, то выходим из приложения
                if (exitTimer)
                {
                    Process.GetCurrentProcess().CloseMainWindow();
                }

                await Task.WhenAny<bool>
                  (
                    popupLayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                popupLayout.IsVisible = !popupLayout.IsVisible;
            }


        }
    }
}