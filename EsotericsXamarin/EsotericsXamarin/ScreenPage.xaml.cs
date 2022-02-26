using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsotericsXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenPage : ContentPage
    {
        public ScreenPage()
        {
            InitializeComponent();
        }

        private async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                var stream = await photo.OpenReadAsync();

                var newStream = File.OpenWrite(newFile);

                await stream.CopyToAsync(newStream);

                image.Source = ImageSource.FromFile(photo.FullPath);
            }

            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        private async void GetPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                image.Source = ImageSource.FromFile(photo.FullPath);
            }

            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
    }
}