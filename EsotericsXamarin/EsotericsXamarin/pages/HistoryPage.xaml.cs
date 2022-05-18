using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsotericsXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {

        private List<ImportImageOriginal> imageOriginals = new List<ImportImageOriginal>();

        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private int countPhoto = 0;
        private int countSelectPhoto = 0;
        private string culture;
        private bool exitTimer = false;
        public HistoryPage()
        {
            

            InitializeComponent();

            againLabel.Text = AppResources.AgainText;

            culture = getCulture(culture);

            

            //действие при нажатие на фото на экране
            TapGestureRecognizer tapImage = new TapGestureRecognizer();
            tapImage.Tapped += (s, e) =>
            {
                selectedImage.IsVisible = false;
                origBut.IsVisible = true;
            };
            selectedImage.GestureRecognizers.Add(tapImage);

            //инициализация тулбара, кнопка очистить
            ToolbarItem toolbar = new ToolbarItem()
            {
                Text = AppResources.ClearText,
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
            };

            //установление действия на кнопку очистить
            toolbar.Clicked += async (s, e) =>
            {
                bool result = await DisplayAlert(null, $"{AppResources.ClearHistoryText}", $"{AppResources.Yes}", $"{AppResources.No}"); ;

                if (result)
                {
                    try
                    {
                        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                        IEnumerable<string> list = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));

                        foreach (string str in list)
                        {
                            if (str.Contains("jpg"))
                            {
                                File.Delete(Path.Combine(folderPath, str));
                            }
                        }

                        File.Delete(Path.Combine(folderPath, "listImage.txt"));
                    }

                    catch (Exception ex)
                    {

                    }

                    EmptyHistory();
                }
            };

            ToolbarItems.Add(toolbar);
        }

        protected override void OnAppearing()
        {
            UpdateList();

            if (imageOriginals.Count == 0)
            {
                EmptyHistory();
                return;
            }

            countSelectPhoto = 0;

            imageOriginals.Reverse();

            countPhotoLabel.Text = countSelectPhoto + "/" + countPhoto;

            ButtonRight_Clicked(new object(), new EventArgs());

            base.OnAppearing();
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

            mainImage.Source = original.image;

            countPhotoLabel.Text = countSelectPhoto + " / " + countPhoto;

            var stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folderPath, original.filename)));

            selectedImage.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });

            CalculatePercent(original.percent);
        }

        private void ButtonTakeImage_Clicked(object sender, EventArgs e)
        {
            selectedImage.IsVisible = true;
            origBut.IsVisible = false;
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

        private async void EmptyHistory()
        {
            await DisplayAlert(null, $"{AppResources.EmptyHistoryText}", "OK");
            await Shell.Current.GoToAsync("//Screen");
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