using Microcharts;
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
    public partial class ReportsPage : TabbedPage
    {      
        public ReportsPage()
        {
            InitializeComponent();
            BindingContext = new ReportsViewModel(this);
        }
    }
}