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
    public partial class ContractorsPage : TabbedPage
    {
        public ContractorsPage()
        {
            BindingContext = ViewModelLocator.ContractorsViewModel;
            InitializeComponent();
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as ContractorsViewModel;
            ContractorsList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ContractorsList.ItemsSource = _container.ContractorsList;
            else
            {
                ContractorsList.ItemsSource = _container.ContractorsList.Where(i => i.Name.ToLower().Contains(e.NewTextValue) || i.NIP.ToLower().Contains(e.NewTextValue));
            }
            ContractorsList.EndRefresh();
        }
    }
}