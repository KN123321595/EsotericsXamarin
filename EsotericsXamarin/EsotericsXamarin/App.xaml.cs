using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsotericsXamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            //CultureInfo.CurrentCulture = new CultureInfo("en");
            //CultureInfo.CurrentUICulture = new CultureInfo("en");


            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
