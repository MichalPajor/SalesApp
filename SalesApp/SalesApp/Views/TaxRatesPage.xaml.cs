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
    public partial class TaxRatesPage : ContentPage
    {
        public TaxRatesPage()
        {
            InitializeComponent();
            BindingContext = new TaxRatesViewModel();
        }

        private void ChBoxA_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxA.IsChecked)
            {
                TaxA.Text = null;
                TaxA.IsEnabled = false;
            }
            else
            {
                TaxA.IsEnabled = true;
            }
        }

        private void ChBoxB_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxB.IsChecked)
            {
                TaxB.Text = null;
                TaxB.IsEnabled = false;
            }
            else
            {
                TaxB.IsEnabled = true;
            }
        }

        private void ChBoxC_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxC.IsChecked)
            {
                TaxC.Text = null;
                TaxC.IsEnabled = false;
            }
            else
            {
                TaxC.IsEnabled = true;
            }
        }

        private void ChBoxD_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxD.IsChecked)
            {
                TaxD.Text = null;
                TaxD.IsEnabled = false;
            }
            else
            {
                TaxD.IsEnabled = true;
            }
        }

        private void ChBoxE_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxE.IsChecked)
            {
                TaxE.Text = null;
                TaxE.IsEnabled = false;
            }
            else
            {
                TaxE.IsEnabled = true;
            }
        }

        private void ChBoxF_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxF.IsChecked)
            {
                TaxF.Text = null;
                TaxF.IsEnabled = false;
            }
            else
            {
                TaxF.IsEnabled = true;
            }
        }

        private void ChBoxG_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ChBoxG.IsChecked)
            {
                TaxG.Text = null;
                TaxG.IsEnabled = false;
            }
            else
            {
                TaxG.IsEnabled = true;
            }
        }
    }
}