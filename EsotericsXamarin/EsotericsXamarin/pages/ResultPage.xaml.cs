using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsotericsXamarin.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        private List<ImportImageOriginal> imageOriginals = new List<ImportImageOriginal>();

        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private int countPhoto = 0;
        private int countSelectPhoto = 1;
        public ResultPage()
        {
            InitializeComponent();           

            ImportImageOriginal original = new ImportImageOriginal(ScreenPage.original.name, ScreenPage.original.desc, ScreenPage.original.image, ScreenPage.fileName);

            titleLabel.Text = original.name;
            descriptionLabel.Text = original.desc;
            mainImage.Source = original.image;

            var stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folderPath, original.filename)));

            selectedImage.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });

            UpdateList();

            AddElement(original);

            imageOriginals.Reverse();

            countPhotoLabel.Text = countSelectPhoto + "/" + countPhoto;


            //действие при нажатие на фото на экране
            TapGestureRecognizer tapImage = new TapGestureRecognizer();
            tapImage.Tapped += (s, e) =>
            {
                if (!selectedImage.IsVisible)
                {                                 
                    selectedImage.IsVisible = true;
                }

                else
                {
                    selectedImage.IsVisible = false;
                }
            };
            selectedImage.GestureRecognizers.Add(tapImage);

            ////действие при нажатие на описание 
            //TapGestureRecognizer tapDesc = new TapGestureRecognizer();
            //tapImage.Tapped += async (s, e) =>
            //{
            //    await DisplayAlert(null, descriptionLabel.Text, "OK");
            //};
            //descriptionLabel.GestureRecognizers.Add(tapDesc);

        }

        private void UpdateList()
        {
            try
            {
                
                if (File.Exists(Path.Combine(folderPath, "listImage.txt")))
                {

                    imageOriginals = JsonConvert.DeserializeObject<List<ImportImageOriginal>>(File.ReadAllText(Path.Combine(folderPath, "listImage.txt")));

                    countPhoto = imageOriginals.Count;

                }

            }

            catch (Exception ex)
            {

            }
        }

        private void AddElement(ImportImageOriginal original)
        {
            try
            {
                imageOriginals.Add(original);

                string json = JsonConvert.SerializeObject(imageOriginals);

                File.WriteAllText(Path.Combine(folderPath, "listImage.txt"), json);

                countPhoto++;
            }

            catch(Exception ex)
            {

            }
        }

        private void ButtonLeft_Clicked(object sender, EventArgs e)
        {
            if (countSelectPhoto == 1)
                return;

            countSelectPhoto--;

            SelectPageInformation(imageOriginals[countSelectPhoto - 1]);


  
        }

        private void ButtonRight_Clicked(object sender, EventArgs e)
        {
            if (countSelectPhoto == imageOriginals.Count)
                return;

            countSelectPhoto++;

            SelectPageInformation(imageOriginals[countSelectPhoto - 1]);

        }

        private void SelectPageInformation(ImportImageOriginal original)
        {
            titleLabel.Text = original.name;
            descriptionLabel.Text = original.desc;
            mainImage.Source = original.image;

            countPhotoLabel.Text = countSelectPhoto + "/" + countPhoto;

            var stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folderPath, original.filename)));

            selectedImage.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });
        }

        private void ButtonTakeImage_Clicked(object sender, EventArgs e)
        {
            selectedImage.IsVisible = true;
        }

        private async void descriptionLabel_Focused(object sender, FocusEventArgs e)
        {
            await DisplayAlert(null, descriptionLabel.Text, "OK");
        }
    }


}