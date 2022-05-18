using System;
using System.Collections.Generic;
using System.IO;
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

            regLabel.Text = AppResources.RegistrationText;
            emailEntry.Placeholder = AppResources.EmailText;
            passwordEntry.Placeholder = AppResources.PasswordText;
            telephoneEntry.Placeholder = AppResources.PhoneNumberText;
            regBut.Text = AppResources.RegistrationText;
            logInLabel.Text = AppResources.logInText;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        private void Button_Reg_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ScreenPage());


            
            
        }
    }
}