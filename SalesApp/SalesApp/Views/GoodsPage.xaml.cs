using SalesApp.Helpers;
using SalesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace SalesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodsPage : TabbedPage
    {
        private GoodsViewModel vm;

        public GoodsPage()
        {
            InitializeComponent();
            BindingContext = vm = ViewModelLocator.GoodsViewModel;
            CurrentPageChanged += CurrentPageHasChanged;
            vm.LoadTaxes();
            vm.LoadUnits();
        }

        private void CurrentPageHasChanged(object sender, EventArgs e)
        {
            PickUnit.SelectedIndex = 0;
            PickTax.SelectedIndex = 0;
        }
        
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as GoodsViewModel;
            GoodsList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                GoodsList.ItemsSource = _container.GoodsList;
            else
            {
                GoodsList.ItemsSource = _container.GoodsList.Where(i => i.Name.ToLower().Contains(e.NewTextValue));
            }
            GoodsList.EndRefresh();
        }
    }
}