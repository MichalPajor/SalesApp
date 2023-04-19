using SalesApp.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SalesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodsEditPage : ContentPage
    {
        public GoodsEditPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.GoodsViewModel;
        }
    }
}