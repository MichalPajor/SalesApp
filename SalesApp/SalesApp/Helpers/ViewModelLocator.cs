using SalesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Helpers
{
    public static class ViewModelLocator
    {
        public static GoodsViewModel _GoodsViewModel = new GoodsViewModel();
        public static GoodsViewModel GoodsViewModel
        {
            get
            {
                return _GoodsViewModel;
            }
        }
        public static SaleViewModel _SaleViewModel = new SaleViewModel();
        public static SaleViewModel SaleViewModel
        {
            get
            {
                return _SaleViewModel;
            }
        }
        public static ContractorsViewModel _ContractorsViewModel = new ContractorsViewModel();
        public static ContractorsViewModel ContractorsViewModel
        {
            get
            {
                return _ContractorsViewModel;
            }
        }
    }
}
