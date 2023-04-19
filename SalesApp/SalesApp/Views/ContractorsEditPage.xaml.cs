using SalesApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SalesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractorsEditPage : ContentPage
    {
        public ContractorsEditPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.ContractorsViewModel;
        }
    }
}