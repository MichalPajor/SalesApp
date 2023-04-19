using Acr.UserDialogs;
using SalesApp.Effects;
using SalesApp.Models;
using SalesApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class GoodsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int _SelectedItemIndex;
        public int SelectedItemIndex 
        {
            get
            {
                return _SelectedItemIndex;
            }
            set
            {
                if (_SelectedItemIndex != value)
                {
                    _SelectedItemIndex = value;
                    OnPropertyChanged("SelectedItemIndex");
                }
            }
        }

        private List<Units> _UnitsList;
        public List<Units> UnitsList 
        {
            get
            {
                return _UnitsList;
            }
            set
            {
                if (_UnitsList != value)
                {
                    _UnitsList = value;
                    OnPropertyChanged("UnitsList");                  
                }
            }
        }

        private List<TaxRates> _TaxList;
        public List<TaxRates> TaxList
        {
            get
            {
                return _TaxList;
            }
            set
            {
                if (_TaxList != value)
                {
                    _TaxList = value;
                    OnPropertyChanged("TaxList");
                }
            }
        }
        public Command SaveCommand { get; set; }
        public Command UpdateCommand { get; set; }
        public Command GoBackCommand { get; set; }
        public Command GoBackFromEditCommand { get; set; }

        private Units _SelectedUnit;
        private TaxRates _SelectedTax;
        public Units SelectedUnit
        {
            get
            {
                return _SelectedUnit;
            }
            set
            {
                if (_SelectedUnit != value)
                {
                    _SelectedUnit = value;
                    OnPropertyChanged("SelectedUnit");
                }
            }
        }
        public TaxRates SelectedTax
        {
            get
            {
                return _SelectedTax;
            }
            set
            {
                if (_SelectedTax != value)
                {
                    _SelectedTax = value;
                    OnPropertyChanged("SelectedTax");
                }
            }
        }
      
        private string _ProductFullNameTxt;
        private string _ProductValueTxt;
        public string ProductFullNameTxt
        {
            get
            {
                return _ProductFullNameTxt;
            }
            set
            {
                if (_ProductFullNameTxt != value)
                {
                    _ProductFullNameTxt = value;
                    OnPropertyChanged("ProductFullNameTxt");
                }
            }
        }
        public string ProductValueTxt
        {
            get
            {
                return _ProductValueTxt;
            }
            set
            {
                if (_ProductValueTxt != value)
                {
                    if (value == ",")
                        value = "";
                    if (value != "" && ProductValueTxt != null && value.Length > ProductValueTxt.Length)
                    {

                        if(value.Contains(",") || value.Contains("."))
                        {
                            value = value.Replace('.', ',');
                            if (ProductValueTxt.Contains(",") && value[value.Length - 1] == ',')
                            {
                                value = value.Substring(0, value.Length - 1);
                            }
                            try
                            {
                                value = value.Substring(0, value.IndexOf(',') + 3);
                            }
                            catch
                            {
                                value = value.Substring(0, value.Length);
                            }                                       
                        }                
                    }
                    _ProductValueTxt = value;
                    OnPropertyChanged("ProductValueTxt");
                }
            }
        }
        private ObservableCollection<Goods> _GoodsList;
        public ObservableCollection<Goods> GoodsList
        {
            get
            {
                return _GoodsList;
            }
            set
            {
                if (_GoodsList != value)
                {
                    _GoodsList = value;
                    OnPropertyChanged("GoodsList");
                }
            }
        }
        private Color _BackgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                if (_BackgroundColor != value)
                {
                    _BackgroundColor = value;
                    OnPropertyChanged("BackgroundColor");
                }
            }
        }
        private Color _LabelTextColor;
        public Color LabelTextColor
        {
            get
            {
                return _LabelTextColor;
            }
            set
            {
                if (_LabelTextColor != value)
                {
                    _LabelTextColor = value;
                    OnPropertyChanged("LabelTextColor");
                }
            }
        }
        private Color _FieldsTextColor;
        public Color FieldsTextColor
        {
            get
            {
                return _FieldsTextColor;
            }
            set
            {
                if (_FieldsTextColor != value)
                {
                    _FieldsTextColor = value;
                    OnPropertyChanged("FieldsTextColor");
                }
            }
        }
        private Color _FieldsBackgroundColor;
        public Color FieldsBackgroundColor
        {
            get
            {
                return _FieldsBackgroundColor;
            }
            set
            {
                if (_FieldsBackgroundColor != value)
                {
                    _FieldsBackgroundColor = value;
                    OnPropertyChanged("FieldsBackgroundColor");
                }
            }
        }
        private Color _FieldsBorderColor;
        public Color FieldsBorderColor
        {
            get
            {
                return _FieldsBorderColor;
            }
            set
            {
                if (_FieldsBorderColor != value)
                {
                    _FieldsBorderColor = value;
                    OnPropertyChanged("FieldsBorderColor");
                }
            }
        }

        private Goods editedGood;

        public ICommand ItemClickCommand
        {
            get
            {
                return new Command(async (item) =>
                {
                    var clicked = item as Goods;
                    editedGood = await App.SQLiteDb.ReadGoodByName(clicked.Name);
                    var config = new ActionSheetConfig
                    {
                        Cancel = new ActionSheetOption("Anuluj"),
                        Title = $"{clicked.Name} - opcje"
                    };

                    config.Add("Usuń", new Action(async () =>
                    {
                        if (await UserDialogs.Instance.ConfirmAsync($"Czy na pewno usunąć {clicked.Name}? \n", "Usuń", "Tak", "Nie"))
                        {
                            await App.SQLiteDb.DeleteGood(clicked);
                            GoodsList.Remove(clicked);
                            UserDialogs.Instance.Toast("Usunięto");
                        }
                    }));

                    config.Add("Edytuj", new Action(async () =>
                    {
                        ProductFullNameTxt = clicked.Name;
                        ProductValueTxt = clicked.Value;
                        SelectedTax = TaxList.Find(i => i.Id == clicked.TaxRatesId);
                        SelectedUnit = UnitsList.Find(i => i.Id == clicked.UnitsId);                      
                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new GoodsEditPage()));

                    }));

                    UserDialogs.Instance.ActionSheet(config);
                    
                });
            }
        }
        public GoodsViewModel()
        {
            SetTheme();
            ReadAllGoods();
            LoadUnits();
            Task.Run(async () => await LoadTaxes());
            GoBackCommand = new Command(async () => await GoBack());
            GoBackFromEditCommand = new Command(async () => await GoBackFromEdit());
            SaveCommand = new Command(
                 execute: async () =>
                 {
                     if(ProductFullNameTxt == "" || ProductFullNameTxt == null || SelectedTax == null || SelectedUnit == null)
                     {
                         UserDialogs.Instance.Toast("Nazwa nie może pozostać pusta. Jednostka oraz stawka VAT muszą być wybrane.");
                         return;
                     }
                     //Check value
                     if(ProductValueTxt != null && ProductValueTxt != "")
                     {
                         double value;
                         try
                         {
                             value = Double.Parse(ProductValueTxt.Replace('.',','));
                         }
                         catch
                         {
                             UserDialogs.Instance.Toast("Przekroczono maksymalną wartość pozycji 999999 lub wprowadzono niedozwolone znaki.");
                             return;
                         }
                         if (value > 999999)
                         {
                             UserDialogs.Instance.Toast("Maksymalna wartość pozycji to 999999");
                             return;
                         }
                         if (value < 0)
                         {
                             UserDialogs.Instance.Toast("Minimalna wartość pozycji to 0");
                             return;
                         }
                     }
                     
                     //Check characters
                     byte[] bytes = Encoding.GetEncoding(1250).GetBytes(ProductFullNameTxt);
                     var hexCodes = BitConverter.ToString(bytes);
                     if (hexCodes.Contains("80") || hexCodes.Contains("8A") || hexCodes.Contains("7F") || hexCodes.Contains("81") ||
                     hexCodes.Contains("82") || hexCodes.Contains("83") || hexCodes.Contains("84") || hexCodes.Contains("85") || hexCodes.Contains("86") ||
                     hexCodes.Contains("87") || hexCodes.Contains("88") || hexCodes.Contains("89") || hexCodes.Contains("8B") || hexCodes.Contains("8D") ||
                     hexCodes.Contains("8E") || hexCodes.Contains("90") || hexCodes.Contains("91") || hexCodes.Contains("92") || hexCodes.Contains("93") ||
                     hexCodes.Contains("94") || hexCodes.Contains("95") || hexCodes.Contains("96") || hexCodes.Contains("97") || hexCodes.Contains("98") ||
                     hexCodes.Contains("99") || hexCodes.Contains("9A") || hexCodes.Contains("9B") || hexCodes.Contains("9D") || hexCodes.Contains("9E") ||
                     hexCodes.Contains("A0") || hexCodes.Contains("A2") || hexCodes.Contains("A4") || hexCodes.Contains("A6") || hexCodes.Contains("A7") ||
                     hexCodes.Contains("A8") || hexCodes.Contains("A9") || hexCodes.Contains("AA") || hexCodes.Contains("AB") || hexCodes.Contains("AC") ||
                     hexCodes.Contains("AD") || hexCodes.Contains("AE") || hexCodes.Contains("B0") || hexCodes.Contains("B1") || hexCodes.Contains("B2") ||
                     hexCodes.Contains("B4") || hexCodes.Contains("B5") || hexCodes.Contains("B6") || hexCodes.Contains("B7") || hexCodes.Contains("B8") ||
                     hexCodes.Contains("BA") || hexCodes.Contains("BB") || hexCodes.Contains("BC") || hexCodes.Contains("BD") || hexCodes.Contains("BE") ||
                     hexCodes.Contains("C0") || hexCodes.Contains("C1") || hexCodes.Contains("C2") || hexCodes.Contains("C3") || hexCodes.Contains("C4") ||
                     hexCodes.Contains("C5") || hexCodes.Contains("C7") || hexCodes.Contains("C8") || hexCodes.Contains("C9") || hexCodes.Contains("CB") ||
                     hexCodes.Contains("CC") || hexCodes.Contains("CD") || hexCodes.Contains("CE") || hexCodes.Contains("CF") || hexCodes.Contains("D0") ||
                     hexCodes.Contains("D2") || hexCodes.Contains("D4") || hexCodes.Contains("D5") || hexCodes.Contains("D6") || hexCodes.Contains("D7") ||
                     hexCodes.Contains("D8") || hexCodes.Contains("D9") || hexCodes.Contains("DA") || hexCodes.Contains("DB") || hexCodes.Contains("DC") ||
                     hexCodes.Contains("DD") || hexCodes.Contains("DE") || hexCodes.Contains("DF") || hexCodes.Contains("E0") || hexCodes.Contains("E1") ||
                     hexCodes.Contains("E2") || hexCodes.Contains("E3") || hexCodes.Contains("E4") || hexCodes.Contains("E6") || hexCodes.Contains("E7") ||
                     hexCodes.Contains("E8") || hexCodes.Contains("E9") || hexCodes.Contains("EB") || hexCodes.Contains("EC") || hexCodes.Contains("ED") ||
                     hexCodes.Contains("EE") || hexCodes.Contains("EF") || hexCodes.Contains("F0") || hexCodes.Contains("F2") || hexCodes.Contains("F4") ||
                     hexCodes.Contains("F5") || hexCodes.Contains("F6") || hexCodes.Contains("F7") || hexCodes.Contains("F8") || hexCodes.Contains("F9") ||
                     hexCodes.Contains("FA") || hexCodes.Contains("FB") || hexCodes.Contains("FC") || hexCodes.Contains("FD") || hexCodes.Contains("FE") ||
                     hexCodes.Contains("FF"))
                     {
                         UserDialogs.Instance.Toast("Nazwa towaru zawiera niedozwolone znaki!");
                         return;
                     }

                     if (ProductFullNameTxt.Length > 30)
                     {
                         UserDialogs.Instance.Toast("Nazwa towaru zbyt długa");
                     }
                     else
                     {
                         var goodFromDb = await App.SQLiteDb.ReadGoodByName(ProductFullNameTxt);
                         if (goodFromDb is null)
                         {
                             Goods good = new Goods
                             {
                                 Name = ProductFullNameTxt,
                                 Value = (ProductValueTxt != null) ? ProductValueTxt.Replace('.',',') : null,
                                 UnitsId = SelectedUnit.Id,
                                 TaxRatesId = SelectedTax.Id
                             };
                             await App.SQLiteDb.InsertGoods(good);
                             await App.SQLiteDb.UpdateGoodsWithChildren(good);
                             UserDialogs.Instance.Toast("Zapisano pomyślnie");
                             GoodsList.Add(good);

                             ProductFullNameTxt = "";
                             ProductValueTxt = "";
                         }
                         else
                         {
                             UserDialogs.Instance.Toast("Towar o podanej nazwie już istnieje. Stwórz nowy");
                         }
                     }                   
                 },
                 canExecute: () =>
                 {
                     return true;
                 });

            UpdateCommand = new Command(
                execute: async () =>
                {
                    if(editedGood is null)
                    {
                        UserDialogs.Instance.Toast("Podany towar już nie istnieje w bazie");
                    }
                    else
                    {
                        if (ProductValueTxt != null && ProductValueTxt != "")
                        {

                            double value;
                            try
                            {
                                value = Double.Parse(ProductValueTxt.Replace('.', ','));
                            }
                            catch
                            {
                                UserDialogs.Instance.Toast("Przekroczono maksymalną wartość pozycji 999999 lub wprowadzono niedozwolone znaki.");
                                return;
                            }
                            if (value > 999999)
                            {
                                UserDialogs.Instance.Toast("Maksymalna wartość pozycji to 999999");
                                return;
                            }
                            if (value < 0)
                            {
                                UserDialogs.Instance.Toast("Minimalna wartość pozycji to 0");
                                return;
                            }
                        }

                        //Check characters
                        byte[] bytes = Encoding.GetEncoding(1250).GetBytes(ProductFullNameTxt);
                        var hexCodes = BitConverter.ToString(bytes);
                        if (hexCodes.Contains("80") || hexCodes.Contains("8A") || hexCodes.Contains("7F") || hexCodes.Contains("81") ||
                        hexCodes.Contains("82") || hexCodes.Contains("83") || hexCodes.Contains("84") || hexCodes.Contains("85") || hexCodes.Contains("86") ||
                        hexCodes.Contains("87") || hexCodes.Contains("88") || hexCodes.Contains("89") || hexCodes.Contains("8B") || hexCodes.Contains("8D") ||
                        hexCodes.Contains("8E") || hexCodes.Contains("90") || hexCodes.Contains("91") || hexCodes.Contains("92") || hexCodes.Contains("93") ||
                        hexCodes.Contains("94") || hexCodes.Contains("95") || hexCodes.Contains("96") || hexCodes.Contains("97") || hexCodes.Contains("98") ||
                        hexCodes.Contains("99") || hexCodes.Contains("9A") || hexCodes.Contains("9B") || hexCodes.Contains("9D") || hexCodes.Contains("9E") ||
                        hexCodes.Contains("A0") || hexCodes.Contains("A2") || hexCodes.Contains("A4") || hexCodes.Contains("A6") || hexCodes.Contains("A7") ||
                        hexCodes.Contains("A8") || hexCodes.Contains("A9") || hexCodes.Contains("AA") || hexCodes.Contains("AB") || hexCodes.Contains("AC") ||
                        hexCodes.Contains("AD") || hexCodes.Contains("AE") || hexCodes.Contains("B0") || hexCodes.Contains("B1") || hexCodes.Contains("B2") ||
                        hexCodes.Contains("B4") || hexCodes.Contains("B5") || hexCodes.Contains("B6") || hexCodes.Contains("B7") || hexCodes.Contains("B8") ||
                        hexCodes.Contains("BA") || hexCodes.Contains("BB") || hexCodes.Contains("BC") || hexCodes.Contains("BD") || hexCodes.Contains("BE") ||
                        hexCodes.Contains("C0") || hexCodes.Contains("C1") || hexCodes.Contains("C2") || hexCodes.Contains("C3") || hexCodes.Contains("C4") ||
                        hexCodes.Contains("C5") || hexCodes.Contains("C7") || hexCodes.Contains("C8") || hexCodes.Contains("C9") || hexCodes.Contains("CB") ||
                        hexCodes.Contains("CC") || hexCodes.Contains("CD") || hexCodes.Contains("CE") || hexCodes.Contains("CF") || hexCodes.Contains("D0") ||
                        hexCodes.Contains("D2") || hexCodes.Contains("D4") || hexCodes.Contains("D5") || hexCodes.Contains("D6") || hexCodes.Contains("D7") ||
                        hexCodes.Contains("D8") || hexCodes.Contains("D9") || hexCodes.Contains("DA") || hexCodes.Contains("DB") || hexCodes.Contains("DC") ||
                        hexCodes.Contains("DD") || hexCodes.Contains("DE") || hexCodes.Contains("DF") || hexCodes.Contains("E0") || hexCodes.Contains("E1") ||
                        hexCodes.Contains("E2") || hexCodes.Contains("E3") || hexCodes.Contains("E4") || hexCodes.Contains("E6") || hexCodes.Contains("E7") ||
                        hexCodes.Contains("E8") || hexCodes.Contains("E9") || hexCodes.Contains("EB") || hexCodes.Contains("EC") || hexCodes.Contains("ED") ||
                        hexCodes.Contains("EE") || hexCodes.Contains("EF") || hexCodes.Contains("F0") || hexCodes.Contains("F2") || hexCodes.Contains("F4") ||
                        hexCodes.Contains("F5") || hexCodes.Contains("F6") || hexCodes.Contains("F7") || hexCodes.Contains("F8") || hexCodes.Contains("F9") ||
                        hexCodes.Contains("FA") || hexCodes.Contains("FB") || hexCodes.Contains("FC") || hexCodes.Contains("FD") || hexCodes.Contains("FE") ||
                        hexCodes.Contains("FF"))
                        {
                            UserDialogs.Instance.Toast("Nazwa towaru zawiera niedozwolone znaki!");
                            return;
                        }

                        if (ProductFullNameTxt.Length > 30)
                        {
                            UserDialogs.Instance.Toast("Nazwa towaru zbyt długa");
                        }

                        else
                        {
                            editedGood.Name = ProductFullNameTxt;
                            editedGood.Value = (ProductValueTxt != null) ? ProductValueTxt.Replace('.', ',') : null;
                            editedGood.UnitsId = SelectedUnit.Id;
                            editedGood.TaxRatesId = SelectedTax.Id;

                            await App.SQLiteDb.UpdateGood(editedGood);
                            await App.SQLiteDb.UpdateGoodsWithChildren(editedGood);
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                            UserDialogs.Instance.Toast("Edytowano pomyślnie");
                            ReadAllGoods();
                            ClearFieldsAfterEdit();
                        }

                    }
                    
                },
                canExecute: () =>
                {
                    return true;
                });
        }
        private void SetTheme()
        {
            if (Theme.IsDarkMode)
            {
                BackgroundColor = DarkTheme.BackgroundColor;
                LabelTextColor = DarkTheme.LabelTextColor;
                FieldsTextColor = DarkTheme.FieldsTextColor;
                FieldsBackgroundColor = DarkTheme.FieldsBackgroundColor;
                FieldsBorderColor = DarkTheme.FieldsBorderColor;
            }
            else
            {
                BackgroundColor = LightTheme.BackgroundColor;
                LabelTextColor = LightTheme.LabelTextColor;
                FieldsTextColor = LightTheme.FieldsTextColor;
                FieldsBackgroundColor = LightTheme.FieldsBackgroundColor;
                FieldsBorderColor = LightTheme.FieldsBorderColor;
            }
        }
        private Task GoBackFromEdit()
        {
            ClearFieldsAfterEdit();
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void ClearFieldsAfterEdit()
        {
            ProductFullNameTxt = "";
            ProductValueTxt = "";
            SelectedTax = TaxList[0];
            SelectedUnit = UnitsList[0];
        }
        public async void LoadUnits()
        {
            UnitsList = await App.SQLiteDb.ReadAllUnits();
        }
        public async Task<List<TaxRates>> LoadTaxes()
        {
            List<TaxRates> taxRates = await App.SQLiteDb.ReadAllTaxRates();
            await Task.Delay(500);
            taxRates.RemoveAll(s =>s.Value == 99.99);
            foreach(var item in taxRates)
            {
                if (item.Value == 98.99)
                {
                    item.Name = $"{item.Name}(Zw)";
                }
                else
                {
                    item.Name = $"{item.Name}({item.Value}%)";
                }
            }
            return TaxList = taxRates;
        }
        private async void ReadAllGoods()
        {
            GoodsList = new ObservableCollection<Goods>(await App.SQLiteDb.ReadAllGoods());
        }
        private Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }

    }
}
