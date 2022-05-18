using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsotericsXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutPage : ContentPage
    {
        public LogoutPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {

            bool result = await DisplayAlert(null, $"{AppResources.LogoutPopup}", $"{AppResources.Yes}", $"{AppResources.No}");

            if (result)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }

            else
                await Shell.Current.GoToAsync("//Screen");

            base.OnAppearing();
        }
    }
}