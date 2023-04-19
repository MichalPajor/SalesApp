using SalesApp.Helpers;
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
    public partial class SalePage : TabbedPage
    {
        public SalePage()
        {
            InitializeComponent();
            //BindingContext = new SaleViewModel();
            ContentPageHelper.salePage = this;
            BindingContext = ViewModelLocator.SaleViewModel;
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as SaleViewModel;


            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ProductsList.ItemsSource = _container.ProductsList;
            else
            {
                ProductsList.ItemsSource = _container.ProductsList.Where(i => i.Name.ToLower().Contains(e.NewTextValue));
            }

        }

    }
}