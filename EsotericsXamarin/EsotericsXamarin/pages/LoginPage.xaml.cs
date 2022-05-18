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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            //действие при нажатие на надпись forgot password
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ChangePasswordPage());
            };
            forgotPasswordLabel.GestureRecognizers.Add(tap);

            loginLabel.Text = AppResources.loginText2;
            loginBut.Text = AppResources.loginText2;
            emailEntry.Placeholder = AppResources.EmailText;
            pasEntry.Placeholder = AppResources.PasswordText;
            forgotPasswordLabel.Text = AppResources.ForgotPasswordText;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
            
        }
    }
}