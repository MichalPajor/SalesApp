using SalesApp.Effects;
using SalesApp.Models;
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
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitData();
            InitializeComponent();
            Fade();
        }

        private async void InitData()
        {
            var invoiceNum = await App.SQLiteDb.ReadAppSetting("InvoiceNum");
            var sysNum = await App.SQLiteDb.ReadAppSetting("SysNum");
            var theme = await App.SQLiteDb.ReadAppSetting("DarkTheme");

            if (theme == null)
            {
                AppSettings invoiceNumSetting = new AppSettings()
                {
                    Name = "DarkTheme",
                    Value = "false"
                };
                App.SQLiteDb.InsertAppSetting(invoiceNumSetting);
            }
            else
            {
                Theme.IsDarkMode = Convert.ToBoolean(theme.Value) ? true : false;
            }
            if (invoiceNum == null)
            {
                AppSettings invoiceNumSetting = new AppSettings()
                {
                    Name = "InvoiceNum",
                    Value = "1"
                };
                App.SQLiteDb.InsertAppSetting(invoiceNumSetting);
            }
            if (sysNum == null)
            {
                AppSettings invoiceNumSetting = new AppSettings()
                {
                    Name = "SysNum",
                    Value = "1"
                };
                App.SQLiteDb.InsertAppSetting(invoiceNumSetting);
            }
        }

        public async Task Fade()
        {
            var image = new Image { Source = "startImage.png" };
            image.Opacity = 0;
            await image.FadeTo(1, 2000);
            Application.Current.MainPage = new ConnectPage();
        }
    }
}