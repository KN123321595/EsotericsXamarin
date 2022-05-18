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
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();

            oldPasEntry.Placeholder = AppResources.OldPasswordText;
            newPasEntry.Placeholder = AppResources.NewPasswordText;
            applyBut.Text = AppResources.ApplyText;
        }
    }
}