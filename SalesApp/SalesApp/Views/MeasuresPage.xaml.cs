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
    public partial class MeasuresPage : TabbedPage
    {
        public MeasuresPage()
        {
            InitializeComponent();
            BindingContext = new MeasuresViewModel();
        }

        void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as MeasuresViewModel;
            UnitsList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                UnitsList.ItemsSource = _container.UnitsList;
            else
            {
                UnitsList.ItemsSource = _container.UnitsList.Where(i => i.Name.ToLower().Contains(e.NewTextValue));
            }
            UnitsList.EndRefresh();
        }
    }
}