using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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


        private string culture;
        private int alignment;

        public ResultPage()
        {
            InitializeComponent();

            culture = getCulture(culture);

            alignment = (int) Math.Round(ScreenPage.percent * 10, 0, MidpointRounding.AwayFromZero);

            CalculatePercent(alignment);

            ImportImageOriginal original = new ImportImageOriginal(ScreenPage.original.name, ScreenPage.original.name_en, ScreenPage.original.name_ger, 
                ScreenPage.original.desc, ScreenPage.original.desc_en, ScreenPage.original.desc_ger,
                ScreenPage.original.sources, ScreenPage.original.image, ScreenPage.fileName, alignment);


            mainImage.Source = original.image;           

            if (culture == "ru")
            {
                titleLabel.Text = original.name;
                descriptionLabel.Text = original.desc;              
            }

            else if (culture == "en")
            {
                titleLabel.Text = original.name_en;
                descriptionLabel.Text = original.desc_en;
            }

            else if (culture == "de")
            {
                titleLabel.Text = original.name_ger;
                descriptionLabel.Text = original.desc_ger;
            }


            var stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folderPath, original.filename)));

            selectedImage.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });

            UpdateList();

            AddElement(original);

            imageOriginals.Reverse();


            //действие при нажатие на фото на экране
            TapGestureRecognizer tapImage = new TapGestureRecognizer();
            tapImage.Tapped += (s, e) =>
            {
                selectedImage.IsVisible = false;
                origBut.IsVisible = true;
            };
            selectedImage.GestureRecognizers.Add(tapImage);

            //действие при нажатие на описание 
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

            }

            catch(Exception ex)
            {

            }
        }


        private void ButtonTakeImage_Clicked(object sender, EventArgs e)
        {
            selectedImage.IsVisible = true;
            origBut.IsVisible = false;
        }

        private async void descriptionLabel_Focused(object sender, FocusEventArgs e)
        {
            await DisplayAlert(null, descriptionLabel.Text, "OK");
        }

        private string getCulture(string culture)
        {
            if (AppResources.Lang == "ru")
            {
                culture = "ru";
            }

            else if (AppResources.Lang == "en")
            {
                culture = "en";
            }

            else if (AppResources.Lang == "de")
            {
                culture = "de";
            }

            return culture;
        }

        private void CalculatePercent(int percent)
        {
            if (percent > 0 && percent < 41)
            {
                lvlBut.Text = $"1 lvl\n{percent}%";
            }

            else if (percent > 40 && percent < 71)
            {
                lvlBut.Text = $"2 lvl\n{percent}%";
            }

            else if (percent > 70 && percent < 91)
            {
                lvlBut.Text = $"3 lvl\n{percent}%";
            }

            else if (percent > 90)
            {
                lvlBut.Text = $"4 lvl\n100%";
            }

            lvlBut.IsVisible = true;
        }
    }


}