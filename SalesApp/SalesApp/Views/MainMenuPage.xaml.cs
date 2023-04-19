using Acr.UserDialogs;
using SalesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SalesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        long lastPress;
        public MainMenuPage()
        {
            InitializeComponent();
            BindingContext = new MainMenuViewModel();
        }
        protected override bool OnBackButtonPressed()
        {
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastPress > 2500)
            {
                UserDialogs.Instance.Toast("Wciśnij jeszcze raz by zamknąć aplikację.", new TimeSpan(2));
                lastPress = currentTime;
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return false;
            }
            return true;
        }
    }
}