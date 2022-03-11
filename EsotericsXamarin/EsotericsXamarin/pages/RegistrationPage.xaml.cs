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
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            
            //действие при нажатие на надпись Log-in
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new LoginPage());
            };
            logInLabel.GestureRecognizers.Add(tap);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        private void Button_Reg_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScreenPage());
        }
    }
}