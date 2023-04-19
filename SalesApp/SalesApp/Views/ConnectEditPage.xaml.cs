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
    public partial class ConnectEditPage : ContentPage
    {
        public ConnectEditPage()
        {
            InitializeComponent();
            BindingContext = new ConnectEditViewModel();
        }
    }
}