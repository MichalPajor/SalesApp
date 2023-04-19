using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using SalesApp.Views;
using SalesApp.Helpers;
using System.Collections.Specialized;
using System.Diagnostics;
using SalesApp.Fiscal;
using Newtonsoft.Json.Linq;
using SalesApp.Effects;
using System.Globalization;

namespace SalesApp.ViewModels
{
    public class SaleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Packets packets;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Command CancelReceiptCommand { get; set; }
        public Command ClearReceiptCommand { get; set; }
        public Command SelectionCommand { get; set; }
        public Command DeleteItemCommand { get; set; }
        public Command UpdateItemCommand { get; set; }
        public Command GoBackFromEditCommand { get; set; }
        public Command CheckedChangedCommand { get; set; }
        public Command SetValueByQttyCommand { get; set; }
        public Command PayInvoiceCommand { get; set; }
        public Command OpenListCommand { get; set; }
        public Command PayCommand { get; set; }
        public Command GetInfoFromAPI { get; set; }
        private ObservableCollection<Goods> _ProductsList;
        public ObservableCollection<Goods> ProductsList
        {
            get
            {
                return _ProductsList;
            }
            set
            {
                if (_ProductsList != value)
                {
                    _ProductsList = value;
                    OnPropertyChanged("ProductsList");
                }
            }
        }
        private ObservableCollection<Contractor> _ContractorsList;
        public ObservableCollection<Contractor> ContractorsList
        {
            get
            {
                return _ContractorsList;
            }
            set
            {
                if (_ContractorsList != value)
                {
                    _ContractorsList = value;
                    OnPropertyChanged("ContractorsList");
                }
            }
        }
        private ObservableCollection<object> _MySelectedItems { get; set; }
        public ObservableCollection<object> MySelectedItems
        {
            get
            {
                if (_MySelectedItems is null)
                    _MySelectedItems = new ObservableCollection<object>();
                return _MySelectedItems;
            }
            set
            {
                if (_MySelectedItems != value)
                {
                    _MySelectedItems = value;
                    OnPropertyChanged("MySelectedItems");
                }
            }
        }
        private ObservableCollectionPropertyNotify<ReceiptItem> _ReceiptItems { get; set; }
        public ObservableCollectionPropertyNotify<ReceiptItem> ReceiptItems
        {
            get
            {
                return _ReceiptItems;
            }
            set
            {
                if (_ReceiptItems != value)
                {
                    _ReceiptItems = value;
                    OnPropertyChanged("ReceiptItems");
                }
            }
        }
        public ICommand ProductClickCommand
        {
            get
            {
                return new Command(async (item) =>
                {
                    var clicked = item as ReceiptItem;
                    var originalItem = await App.SQLiteDb.ReadGoodByName(clicked.Name);
                    originalValue = (originalItem.Value==null) ? double.Parse(clicked.Value) : double.Parse(originalItem.Value);
                    ItemDiscount = clicked.Discount.ToString();
                    ItemName = clicked.Name;
                    ItemValue = clicked.Value;
                    ItemQuantity = clicked.Quantity;
                    ItemUnit = clicked.Unit;                    
                    ItemTaxName = clicked.TaxName;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new SaleEditItemPage()));
                    
                   
                });
            }
        }
        public ICommand ContractorClickCommand
        {
            get
            {
                return new Command(async (item) =>
                {
                    var clicked = item as Contractor;
                    InvoiceNIP = clicked.NIP;
                    InvoiceName = clicked.Name;
                    InvoiceStreet = clicked.Street;
                    InvoiceLocalNum = clicked.LocalNum;
                    InvoicePostCode = clicked.PostCode;
                    InvoiceStreetNum = clicked.StreetNum;
                    InvoiceCity = clicked.City;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                });
            }
        }
        private string _SumValue;
        public string SumValue
        {
            get
            {
                return _SumValue;
            }
            set
            {
                if (_SumValue != value)
                {
                    _SumValue = value;
                    OnPropertyChanged("SumValue");
                }
            }
        }
        private string _ReceiptItemsCount;
        public string ReceiptItemsCount
        {
            get
            {
                return _ReceiptItemsCount;
            }
            set
            {
                if (_ReceiptItemsCount != value)
                {
                    _ReceiptItemsCount = value;
                    OnPropertyChanged("ReceiptItemsCount");
                }
            }
        }
        private string _ItemBaseValue;
        public string ItemBaseValue
        {
            get
            {
                return _ItemBaseValue;
            }
            set
            {
                if (_ItemBaseValue != value)
                {
                    if (value == ",")
                        value = "";
                    if (value != "")
                    {
                        if (value != "" && ItemBaseValue != null && value.Length > ItemBaseValue.Length)
                        {

                            if (value.Contains(",") || value.Contains("."))
                            {
                                value = value.Replace('.', ',');
                                if (ItemBaseValue.Contains(",") && value[value.Length - 1] == ',')
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
                    }
                    _ItemBaseValue = value;
                    OnPropertyChanged("ItemBaseValue");
                    
                }
            }
        }
        private bool _ChangeValue;
        public bool ChangeValue
        {
            get
            {
                return _ChangeValue;
            }
            set
            {
                if (_ChangeValue != value)
                {
                    _ChangeValue = value;
                    OnPropertyChanged("ChangeValue");
                }
            }
        }
        private bool _IsPriceEnabled;
        public bool IsPriceEnabled
        {
            get
            {
                return _IsPriceEnabled;
            }
            set
            {
                if (_IsPriceEnabled != value)
                {
                    _IsPriceEnabled = value;
                    OnPropertyChanged("IsPriceEnabled");
                }
            }
        }
        private bool _IsDiscountEnabled;
        public bool IsDiscountEnabled
        {
            get
            {
                return _IsDiscountEnabled;
            }
            set
            {
                if (_IsDiscountEnabled != value)
                {
                    _IsDiscountEnabled = value;
                    OnPropertyChanged("IsDiscountEnabled");
                }
            }
        }
        private bool _IsManualValueVisible;
        public bool IsManualValueVisible
        {
            get
            {
                return _IsManualValueVisible;
            }
            set
            {
                if (_IsManualValueVisible != value)
                {
                    _IsManualValueVisible = value;
                    OnPropertyChanged("IsManualValueVisible");
                }
            }
        }
        private string _DiscountValue;
        public string DiscountValue
        {
            get
            {
                return _DiscountValue;
            }
            set
            {
                if (_DiscountValue != value)
                {
                    _DiscountValue = value;
                    OnPropertyChanged("DiscountValue");
                }
            }
        }
        private int lastCheckedItemIndex = 0;
        private double originalValue;
        private string _ItemName { get; set; }
        public string ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }
        private string _ItemValue { get; set; }
        public string ItemValue
        {
            get
            {
                return _ItemValue;
            }
            set
            {
                if (_ItemValue != value)
                {
                    if (value == ",")
                        value = "";
                    if (value != "")
                    {
                        if (value != "" && ItemValue != null && value.Length > ItemValue.Length)
                        {

                            if (value.Contains(",") || value.Contains("."))
                            {
                                value = value.Replace('.', ',');
                                if (ItemValue.Contains(",") && value[value.Length - 1] == ',')
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
                    }
                    _ItemValue = value;
                    OnPropertyChanged("ItemValue");
                }
            }
        }
        private string _ItemUnit { get; set; }
        public string ItemUnit
        {
            get
            {
                return _ItemUnit;
            }
            set
            {
                if (_ItemUnit != value)
                {
                    _ItemUnit = value;
                    OnPropertyChanged("ItemUnit");
                }
            }
        }
        private string _ItemQuantity { get; set; }
        public string ItemQuantity
        {
            get
            {
                return _ItemQuantity;
            }
            set
            {
                if (_ItemQuantity != value)
                {
                    if (value == ",")
                        value = "";
                    if (value != "")
                    {
                        if (value != "" && ItemQuantity != null && value.Length > ItemQuantity.Length)
                        {

                            if (value.Contains(",") || value.Contains("."))
                            {
                                value = value.Replace('.', ',');
                                if (ItemQuantity.Contains(",") && value[value.Length - 1] == ',')
                                {
                                    value = value.Substring(0, value.Length - 1);
                                }
                                try
                                {
                                    value = value.Substring(0, value.IndexOf(',') + 4);
                                }
                                catch
                                {
                                    value = value.Substring(0, value.Length);
                                }
                            }
                        }
                    }
                    _ItemQuantity = value;
                    OnPropertyChanged("ItemQuantity");
                }
            }
        }
        private string _ItemDiscount { get; set; }
        public string ItemDiscount
        {
            get
            {
                return _ItemDiscount;
            }
            set
            {
                if (_ItemDiscount != value)
                {
                    if (value == ",")
                        value = "";
                    if (value != "")
                    {
                        if (value != "" && ItemDiscount != null && value.Length > ItemDiscount.Length)
                        {

                            if (value.Contains(",") || value.Contains("."))
                            {
                                value = value.Replace('.', ',');
                                if (ItemDiscount.Contains(",") && value[value.Length - 1] == ',')
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
                    }
                    _ItemDiscount = value;
                    OnPropertyChanged("ItemDiscount");
                }
            }
        }
        private string _ItemTaxName { get; set; }
        public string ItemTaxName
        {
            get
            {
                return _ItemTaxName;
            }
            set
            {
                if (_ItemTaxName != value)
                {
                    _ItemTaxName = value;
                    OnPropertyChanged("ItemTaxName");
                }
            }
        }
        private string _InvoiceNIP { get; set; }
        public string InvoiceNIP
        {
            get
            {
                return _InvoiceNIP;
            }
            set
            {
                if (_InvoiceNIP != value)
                {
                    _InvoiceNIP = value;
                    OnPropertyChanged("InvoiceNIP");
                }
            }
        }
        private string _InvoiceName { get; set; }
        public string InvoiceName
        {
            get
            {
                return _InvoiceName;
            }
            set
            {
                if (_InvoiceName != value)
                {
                    _InvoiceName = value;
                    OnPropertyChanged("InvoiceName");
                }
            }
        }
        private string _InvoiceStreet { get; set; }
        public string InvoiceStreet
        {
            get
            {
                return _InvoiceStreet;
            }
            set
            {
                if (_InvoiceStreet != value)
                {
                    _InvoiceStreet = value;
                    OnPropertyChanged("InvoiceStreet");
                }
            }
        }
        private string _InvoiceLocalNum { get; set; }
        public string InvoiceLocalNum
        {
            get
            {
                return _InvoiceLocalNum;
            }
            set
            {
                if (_InvoiceLocalNum != value)
                {
                    _InvoiceLocalNum = value;
                    OnPropertyChanged("InvoiceLocalNum");
                }
            }
        }
        private string _InvoicePostCode { get; set; }
        public string InvoicePostCode
        {
            get
            {
                return _InvoicePostCode;
            }
            set
            {
                if (_InvoicePostCode != value)
                {
                    _InvoicePostCode = value;
                    OnPropertyChanged("InvoicePostCode");
                }
            }
        }
        private string _InvoiceStreetNum { get; set; }
        public string InvoiceStreetNum
        {
            get
            {
                return _InvoiceStreetNum;
            }
            set
            {
                if (_InvoiceStreetNum != value)
                {
                    _InvoiceStreetNum = value;
                    OnPropertyChanged("InvoiceStreetNum");
                }
            }
        }
        private string _InvoiceCity { get; set; }
        public string InvoiceCity
        {
            get
            {
                return _InvoiceCity;
            }
            set
            {
                if (_InvoiceCity != value)
                {
                    _InvoiceCity = value;
                    OnPropertyChanged("InvoiceCity");
                }
            }
        }
        private bool _InvoicePrintCopy { get; set; }
        public bool InvoicePrintCopy
        {
            get
            {
                return _InvoicePrintCopy;
            }
            set
            {
                if (_InvoicePrintCopy != value)
                {
                    _InvoicePrintCopy = value;
                    OnPropertyChanged("InvoicePrintCopy");
                }
            }
        }
        private bool _ApiLoading { get; set; }
        public bool ApiLoading
        {
            get
            {
                return _ApiLoading;
            }
            set
            {
                if (_ApiLoading != value)
                {
                    _ApiLoading = value;
                    OnPropertyChanged("ApiLoading");
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

        public SaleViewModel()
        {
            SetTheme();
            ReadAllProducts();
            ReceiptItems = new ObservableCollectionPropertyNotify<ReceiptItem>();
            SumValue = "0";
            DiscountValue = "0";
            ReceiptItemsCount = "0";
            IsPriceEnabled = true;
            IsDiscountEnabled = false;
            ChangeValue = false;
            packets = new Packets();
            CancelReceiptCommand = new Command(async () => await CancelReceipt());
            OpenListCommand = new Command(async () => await OpenCoontractorsList());
            GoBackFromEditCommand = new Command(async () => await GoBackFromEdit());
            ClearReceiptCommand = new Command(async () => await ClearReceiptAsync());
            DeleteItemCommand =  new Command(() => DeleteItemFromReceipt());        
            UpdateItemCommand = new Command(() => UpdateItem());
            SetValueByQttyCommand = new Command(() => SetValueByQtty());
            CheckedChangedCommand = new Command(() => CheckedChanged());
            PayCommand = new Command(() => PickPrintoutType());
            GetInfoFromAPI = new Command(async () => await GetApiInfo());
            PayInvoiceCommand = new Command(() => {
                if (String.IsNullOrEmpty(InvoiceNIP) || String.IsNullOrEmpty(InvoiceCity) || String.IsNullOrEmpty(InvoiceStreet) || String.IsNullOrEmpty(InvoiceStreetNum) || String.IsNullOrEmpty(InvoicePostCode))
                {
                    UserDialogs.Instance.Toast("Uzupełnij wszystkie wymagane pola.");
                }else if (!ValidateNIP.IsValidNIP(InvoiceNIP))
                {
                    UserDialogs.Instance.Toast("Wprowadzony NIP jest nieprawidłowy.");
                }
                else
                {
                    PaymentMethod(false);
                }
            }
            );
            SelectionCommand = new Command(
            execute: async () =>
            {
                await CreateReceiptAsync();
                SetSumAndDiscountValue();
            },
            canExecute: () =>
            {
                return true;
            });          
        }
        private async Task OpenCoontractorsList()
        {
            ReadAllContractors();
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new PickContractor()));
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
        private void PickPrintoutType()
        {
            if (TCPConnection.SocketConnected())
            {
                if (ReceiptItems.Count != 0)
                {
                    var config = new ActionSheetConfig
                    {
                        Cancel = new ActionSheetOption("Anuluj"),
                        Title = "Typ wydruku"
                    };
                    config.Add("Paragon", new Action( () =>
                    {
                        PaymentMethod(true);
                    }));
                    config.Add("Faktura", new Action(async  () =>
                    {
                        InvoicePrintCopy = true;
                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new InvoicePage()));
                        //PaymentMethod(false);
                    }));
                    UserDialogs.Instance.ActionSheet(config);
                }
                else
                {
                    UserDialogs.Instance.Toast("Nie można dokonać wydruku na pustym koszyku...");
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Brak połączenia z drukarką!");
            }
            
        }


        private void PaymentMethod(bool isReceipt)
        {
            var config = new ActionSheetConfig
            {
                Cancel = new ActionSheetOption("Anuluj"),
                Title = "Metoda płatności"
            };

            config.Add("Gotówka", new Action(async () =>
                    {
                        string input = "";
                        while (input == "" || input == null)
                        {
                            input = await ContentPageHelper.salePage.DisplayPromptAsync("Wprowadź wartość kwoty wpłaconej przez klienta:", "", "Potwierdź", "Anuluj", "Kwota", -1, Keyboard.Telephone, "");
                            if (input != null && (input.Contains('*') || input.Contains('+') || input.Contains('#') || input.Contains('(') || input.Contains(')') || input.Contains('/') || input.Contains('N') ||
                            input.Contains(';') || input.Contains('-') || input.Contains(' ') || input.Equals(",")))
                            {
                                await UserDialogs.Instance.AlertAsync("Wprowadzonob błędną wartość lub niedozwolone znaki!", "Błąd", "Ok");
                                input = "";
                            }
                            
                        }
                        input = input.Replace('.', ',');
                        double sumValue = double.Parse(SumValue);
                        DateTime dateTime = DateTime.Now;

                        if (input != null && input != "")
                        {
                            double inputValue = double.Parse(input);
                            if (inputValue < sumValue)
                            {
                                UserDialogs.Instance.Toast("Wpłata nie może być mniejsza niż wartość paragonu!");
                            }
                            else
                            {                  
                                int position = 1;
                                if (isReceipt)
                                {
                                    TCPConnection.Send(packets.ReceiptCancel());
                                    TCPConnection.Send(packets.StartReceipt(ReceiptItems.Count)); // DRUKOWANIE PARAGONU));
                                    foreach (var product in ReceiptItems)
                                    {
                                        TCPConnection.Send(packets.ReceiptLine(position, (product.Discount == 0) ? 0 : 1, product.Name, product.Quantity.Replace(',','.') + " " + product.Unit, product.TaxName,
                                            ((double.Parse(product.Value) + product.Discount) / double.Parse(product.Quantity)).ToString().Replace(",", "."),
                                            (double.Parse(product.Value) + product.Discount).ToString().Replace(",", "."), product.Discount));
                                        position++;
                                    }
                                    TCPConnection.Send(packets.ReceiptEnd(inputValue, sumValue));
                                }
                                else
                                {
                                    //DRUKOWANIE FAKTURY
                                    var invoiceNum = await App.SQLiteDb.ReadAppSetting("InvoiceNum");
                                    var sysNum = await App.SQLiteDb.ReadAppSetting("SysNum");                             
                                    string l1 = String.IsNullOrEmpty(InvoiceLocalNum)? $"{InvoiceStreet} {InvoiceStreetNum}": $"{InvoiceStreet} {InvoiceStreetNum}/{InvoiceLocalNum}";
                                    string l2 = $"{InvoiceCity} {InvoicePostCode}";
                                    string l3 = $"";
                                    TCPConnection.Send(packets.ReceiptCancel());
                                    TCPConnection.Send(packets.StartInvoice(ReceiptItems.Count, "Gotówka", int.Parse(invoiceNum.Value), int.Parse(sysNum.Value), InvoiceNIP, InvoiceName, InvoicePrintCopy,l1,l2,l3 ));
                                    foreach (var product in ReceiptItems)
                                    {
                                        TCPConnection.Send(packets.ReceiptLine(position, (product.Discount == 0) ? 0 : 1, product.Name, product.Quantity.Replace(',', '.') + " " + product.Unit, product.TaxName,
                                            ((double.Parse(product.Value) + product.Discount) / double.Parse(product.Quantity)).ToString().Replace(",", "."),
                                            (double.Parse(product.Value) + product.Discount).ToString().Replace(",", "."), product.Discount));
                                        position++;
                                    }
                                    TCPConnection.Send(packets.InvoiceEnd(inputValue, sumValue));

                                    //Podniesienie numeru faktury w bazie
                                    int invoiceInt = int.Parse(invoiceNum.Value) + 1;
                                    invoiceNum.Value = invoiceInt.ToString();
                                    await App.SQLiteDb.UpdateAppSetting(invoiceNum);
                                    //Podniesienie numeru systemowego w bazie
                                    int sysInt = int.Parse(sysNum.Value) + 1;
                                    sysNum.Value = sysInt.ToString();
                                    await App.SQLiteDb.UpdateAppSetting(sysNum);
                                    await GoBackFromEdit();
                                }
                                if (inputValue > sumValue)
                                {
                                    UserDialogs.Instance.Alert($"Reszta: {Math.Round((inputValue - sumValue),2)}");
                                }
                                TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                                UserDialogs.Instance.Toast("Drukowanie dokumentu... czekaj", timeSpan);
                                foreach (var item in ReceiptItems)
                                {
                                    Sales product = new Sales()
                                    {
                                        //Date = dateTime.ToString("dd.MM.yyyy"),
                                        Date = dateTime,
                                        ProductName = item.Name,
                                        Value = float.Parse(item.Value),
                                    };
                                    await App.SQLiteDb.InsertSales(product);
                                }
                                await ClearReceiptAsync(true);
                            }

                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Anulowano płatnosć");
                        }

                    }));

            config.Add("Karta", new Action(async () =>
                    {
                        double sumValue = double.Parse(SumValue);
                        DateTime dateTime = DateTime.Now;
                        int position = 1;
                        if (isReceipt)
                        {
                            TCPConnection.Send(packets.ReceiptCancel());
                            TCPConnection.Send(packets.StartReceipt(ReceiptItems.Count)); // DRUKOWANIE PARAGONU
                            foreach (var product in ReceiptItems)
                            {
                                TCPConnection.Send(packets.ReceiptLine(position, (product.Discount == 0) ? 0 : 1, product.Name, product.Quantity.Replace(',', '.') + " " + product.Unit, product.TaxName,
                                    ((double.Parse(product.Value) + product.Discount) / double.Parse(product.Quantity)).ToString().Replace(",", "."),
                                    (double.Parse(product.Value) + product.Discount).ToString().Replace(",", "."), product.Discount));
                                position++;
                            }
                            TCPConnection.Send(packets.EndReceiptForm2(sumValue));
                        }
                        else
                        {
                            var invoiceNum = await App.SQLiteDb.ReadAppSetting("InvoiceNum");
                            var sysNum = await App.SQLiteDb.ReadAppSetting("SysNum");
                            string l1 = String.IsNullOrEmpty(InvoiceLocalNum) ? $"{InvoiceStreet} {InvoiceStreetNum}" : $"{InvoiceStreet} {InvoiceStreetNum}/{InvoiceLocalNum}";
                            string l2 = $"{InvoiceCity} {InvoicePostCode}";
                            string l3 = $"";
                            TCPConnection.Send(packets.ReceiptCancel());
                            TCPConnection.Send(packets.StartInvoice(ReceiptItems.Count, "Karta", int.Parse(invoiceNum.Value), int.Parse(sysNum.Value), InvoiceNIP, InvoiceName, InvoicePrintCopy, l1, l2, l3));
                            foreach (var product in ReceiptItems)
                            {
                                TCPConnection.Send(packets.ReceiptLine(position, (product.Discount == 0) ? 0 : 1, product.Name, product.Quantity.Replace(',', '.') + " " + product.Unit, product.TaxName,
                                    ((double.Parse(product.Value) + product.Discount) / double.Parse(product.Quantity)).ToString().Replace(",", "."),
                                    (double.Parse(product.Value) + product.Discount).ToString().Replace(",", "."), product.Discount));
                                position++;
                            }
                            TCPConnection.Send(packets.InvoiceEnd(sumValue, sumValue));

                            //Podniesienie numeru faktury w bazie
                            int invoiceInt = int.Parse(invoiceNum.Value) + 1;
                            invoiceNum.Value = invoiceInt.ToString();
                            await App.SQLiteDb.UpdateAppSetting(invoiceNum);
                            //Podniesienie numeru systemowego w bazie
                            int sysInt = int.Parse(sysNum.Value) + 1;
                            sysNum.Value = sysInt.ToString();
                            await App.SQLiteDb.UpdateAppSetting(sysNum);
                            await GoBackFromEdit();
                        }
                       

                        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                        UserDialogs.Instance.Toast("Drukowanie dokumentu... czekaj", timeSpan);
                        foreach (var item in ReceiptItems)
                        {
                            Sales product = new Sales()
                            {
                                //Date = dateTime.ToString("dd.MM.yyyy"),
                                Date = dateTime,
                                ProductName = item.Name,
                                Value = float.Parse(item.Value),
                            };
                            await App.SQLiteDb.InsertSales(product);
                        }
                        await ClearReceiptAsync(true);

                    }));

            UserDialogs.Instance.ActionSheet(config);
        }

        private void CheckedChanged()
        {
            if (ChangeValue)
            {
                SetValueByQtty();
                IsManualValueVisible = true;
                ItemBaseValue = originalValue.ToString();
            }
            else
            {
                IsManualValueVisible = false;
                SetValueByQtty();
            }
        }

        private void SetValueByQtty()
        {
            var discount = (ItemDiscount == "") ? "0" : ItemDiscount;
            var quantity = (ItemQuantity == "") ? "1" : ItemQuantity;
            if (quantity != "" && quantity != null && discount != "" && discount != null)
            {
                //ItemValue = (double.Parse(quantity) * originalValue - double.Parse(discount)).ToString("0.00");
                if (ChangeValue && ItemBaseValue != null && ItemBaseValue != "")
                {
                    ItemValue = Math.Round((double.Parse(quantity) * double.Parse(ItemBaseValue)) - double.Parse(discount), 2).ToString();
                }
                else
                {
                    ItemValue = Math.Round((double.Parse(quantity) * originalValue) - double.Parse(discount), 2).ToString();
                }

            }
        }
        private async void UpdateItem()
        {
            ItemValue = (ItemValue == "") ? originalValue.ToString() : ItemValue;
            ItemDiscount = (ItemDiscount == "") ? "0" : ItemDiscount;
            ItemQuantity = (ItemQuantity == "") ? "1" : ItemQuantity;



            double value = 0;
            try
            {
                value = double.Parse(ItemValue.Replace('.', ','));
            }
            catch
            {
                UserDialogs.Instance.Toast("Przekroczono maksymalną wartość pozycji 999999 lub wprowadzono niedozwolone znaki.");
                return;
            }

            double discount = double.Parse(ItemDiscount);
            if(value > 999999)
            {
                UserDialogs.Instance.Toast("Wartośc pozycji nie może być większa niż 999999");
                return;
            }
            if(value < 0) 
            {
                UserDialogs.Instance.Toast("Wartośc pozycji nie może być mniejsza niż 0");
            }
            else
            {
                var updatedItem = ReceiptItems.FirstOrDefault(i => i.Name.Equals(ItemName));
                updatedItem.Quantity = ItemQuantity;
                updatedItem.Value = value.ToString().Replace('.',',');
                updatedItem.Discount = double.Parse(ItemDiscount);  
                await Application.Current.MainPage.Navigation.PopModalAsync();
                ChangeValue = false;
                SetSumAndDiscountValue();
                ReceiptItems.Refresh();
                UserDialogs.Instance.Toast("Zapisano");
            }         
        }
        private async void DeleteItemFromReceipt()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
            foreach (var selected in MySelectedItems)
            {
                var product = selected as Goods;
                if (product.Name.Equals(ItemName))
                {
                    MySelectedItems.Remove(selected);
                    break;
                }
            }
            UserDialogs.Instance.Toast("Usunięto pozycje");
        }
        private async Task ClearReceiptAsync(bool? force = false)
        {
            if (MySelectedItems.Count != 0)
            {
                if ((bool)!force)
                {
                    if (await UserDialogs.Instance.ConfirmAsync("Czy na pewno wyczyścić paragon?", "Wyczyść paragon", "Tak", "Nie"))
                    {
                        MySelectedItems.Clear();
                        ReceiptItems.Clear();
                        lastCheckedItemIndex = 0;
                        SetSumAndDiscountValue();
                    }
                }
                else
                {
                    MySelectedItems.Clear();
                    ReceiptItems.Clear();
                    lastCheckedItemIndex = 0;
                    SetSumAndDiscountValue();
                }
                ReceiptItemsCount = "0";
            }
        }
        private async Task CancelReceipt()
        {
            if(MySelectedItems.Count != 0)
            {
                if (await UserDialogs.Instance.ConfirmAsync("Czy na pewno anulować rozpoczęty paragon?", "Anuluj paragon", "Tak", "Nie"))
                {              
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                   // ViewModelLocator._SaleViewModel = new SaleViewModel();
                }
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
               // ViewModelLocator._SaleViewModel = new SaleViewModel();
            }
            
        }
        private async Task CreateReceiptAsync()
        {
            if(MySelectedItems.Count < ReceiptItems.Count)
            {
                var MyGoodsList = new List<Goods>();
                foreach(var selected in MySelectedItems)
                {
                    var product = selected as Goods;
                    MyGoodsList.Add(product);
                }
                foreach (var item in ReceiptItems)
                {                
                    var match = MyGoodsList.Where(p => p.Id == item.Id);    
                    if(match.Count() == 0)
                    {
                        ReceiptItems.Remove(item);
                        break;
                    }
                }
                lastCheckedItemIndex--;
            }
            else
            {
                var item = MySelectedItems[lastCheckedItemIndex] as Goods;
                string value = (item.Value==null) ? "" : item.Value;
                while(value == "" || value == null)
                {
                    value = await ContentPageHelper.salePage.DisplayPromptAsync("Wprowadź wartość do pozycji z otwartą ceną: "+item.Name +"", "", "Potwierdź", "", "Kwota", -1, Keyboard.Telephone, "");
                    if (value != null && (value.Contains('*') || value.Contains('+') || value.Contains('#') || value.Contains('(') || value.Contains(')') || value.Contains('/') || value.Contains('N') ||
                    value.Contains(';') || value.Contains('-') || value.Contains(' ') || value.Equals(",")))
                    {
                        await UserDialogs.Instance.AlertAsync("Wprowadzonob błędną wartość lub niedozwolone znaki!", "Błąd", "Ok");
                        value = "";
                    }
                    
                }
                value = Math.Round(double.Parse(value.Replace('.', ',')), 2).ToString();
                var tax = await App.SQLiteDb.ReadTaxRatebyId(item.TaxRatesId);
                var unit = await App.SQLiteDb.ReadUnitbyId(item.UnitsId);
                ReceiptItem receiptItem = new ReceiptItem()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Value = value,
                    Quantity = "1",
                    TaxName = tax.Name,
                    Unit = unit.ShortCut
                };
                ReceiptItems.Add(receiptItem);
                lastCheckedItemIndex++;
            }
            ReceiptItemsCount = ReceiptItems.Count.ToString();
        }
        private async Task GoBackFromEdit()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
            ChangeValue = false;
        }
        private void SetSumAndDiscountValue()
        {
            double sum = 0;
            double discount = 0;
            if(ReceiptItems.Count != 0)
            {
                foreach (var item in ReceiptItems)
                {
                    sum += double.Parse(item.Value);
                    discount += item.Discount;
                }
            }
            SumValue = (sum).ToString();
            DiscountValue = discount.ToString();
        }
        private async void ReadAllProducts()
        {
            ProductsList = new ObservableCollection<Goods>(await App.SQLiteDb.ReadAllGoods());
        }
        private async void ReadAllContractors()
        {
            ContractorsList = new ObservableCollection<Contractor>(await App.SQLiteDb.ReadAllContractors());
        }
        private async Task GetApiInfo()
        {
            if (ValidateNIP.IsValidNIP(InvoiceNIP))
            {
                ApiLoading = true;
                //InvoiceNIP = "5220001694";
                var data = await ApiMethods.GetData(InvoiceNIP);
                //var data = "{\"vatError\":false,\"gusError\":false,\"vatErrorMessage\":\"\",\"gusErrorMessage\":\"\",\"gusReportType\":\"BIR11OsPrawna\",\"data\":{\"regon\":\"012499190\",\"nipStatus\":\"\",\"nip\":\"5220001694\",\"name\":\"COMP SPÓŁKA AKCYJNA\",\"shortName\":\"COMP SA\",\"registeredNumber\":\"0000037706\",\"registrationDate\":\"2001-08-24\",\"establishmentDate\":\"1997-05-28\",\"activityStartDate\":\"1997-05-28\",\"regonEntryDate\":\"\",\"registrationAuthoritySymbol\":\"071010060\",\"registrationTypeSymbol\":\"138\",\"basicLegalFormName\":\"OSOBA PRAWNA\",\"detailedLegalFormName\":\"SPÓŁKI AKCYJNE\",\"financingFormName\":\"JEDNOSTKA SAMOFINANSUJĄCA NIE BĘDĄCA JEDNOSTKĄ BUDŻETOWĄ LUB SAMORZĄDOWYM ZAKŁADEM BUDŻETOWYM\",\"ownershipFormName\":\"WŁASNOŚĆ PRYWATNA KRAJOWA POZOSTAŁA\",\"foundingAuthorityName\":\"\",\"registrationAuthorityName\":\"SĄD REJONOWY DLA M.ST.WARSZAWY W WARSZAWIE,XIV WYDZIAŁ GOSPODARCZY KRAJOWEGO REJESTRU SĄDOWEGO\",\"registerTypeName\":\"REJESTR PRZEDSIĘBIORCÓW\",\"localUnitsNumber\":\"3\",\"registerOfRecordsEntryDate\":\"\",\"country\":\"POLSKA\",\"countrySymbol\":\"PL\",\"voivodeship\":\"MAZOWIECKIE\",\"voivodeshipSymbol\":\"14\",\"county\":\"Warszawa\",\"countySymbol\":\"65\",\"district\":\"Włochy\",\"districtSymbol\":\"178\",\"city\":\"Warszawa\",\"citySymbol\":\"0988862\",\"postOfficeLocationName\":\"Warszawa\",\"postOfficeLocationSymbol\":\"0988862\",\"postalCode\":\"02230\",\"street\":\"ul. Jutrzenki\",\"streetSymbol\":\"07699\",\"activitySuspensionDate\":\"\",\"activityResumptionDate\":\"\",\"activityChangeDate\":\"2021-08-10\",\"activityTerminationDate\":\"\",\"regonDeletionDate\":\"\",\"buildingNumber\":\"116\",\"premisesNumber\":\"\",\"unusualLocalization\":\"\",\"phoneNumber\":\"225703800\",\"internalPhoneNumber\":\"\",\"faxNumber\":\"226626371\",\"email\":\"info@comp.com.pl\",\"website\":\"www.comp.com.pl\",\"basicLegalFormSymbol\":\"1\",\"detailedLegalFormSymbol\":\"116\",\"financingFormSymbol\":\"1\",\"ownershipFormSymbol\":\"215\",\"foundingOrganSymbol\":\"\",\"bankruptcyProcessStartDate\":\"\",\"bankruptcyProcessEndDate\":\"\",\"accountNumbers\":[\"67114010100000274544001003\",\"06114010100000274544001034\",\"75114010100000478619001016\",\"38114010100000478619001003\",\"64191011233400161121210001\",\"92114010100000478619001001\",\"50175000090000000000909734\",\"49114010100000274544001036\",\"76114010100000274544001035\",\"61114010100000274544001014\",\"10102010970000750201065499\",\"58175000090000000005841167\",\"24114010100000274544001001\",\"76175000090000000002448947\",\"41175000090000000002448898\",\"65114010100000478619001002\",\"22114010100000274544001037\",\"66175000090000000013779406\",\"56114010100000274544001007\",\"33114010100000274544001033\",\"60102010260000110202268050\",\"77175000090000000039938847\",\"94116022020000000350933375\",\"13175000090000000002275578\",\"05114021050000274544001062\",\"06114021050000478619001007\",\"07102010260000140203570694\",\"95124010371111001081234984\",\"28124010371111001060058471\",\"65114010100000274544001039\",\"80175000090000000039328372\",\"51116022020000000218870395\",\"74116022020000000221001750\",\"44175000090000000011534252\",\"22116022020000000195105626\",\"32114021050000274544001061\",\"54114021050000274544001053\",\"31102010260000180202268035\",\"89114021050000274544001005\",\"92114010100000274544001038\",\"80116022020000000221001448\",\"60114010100000274544001032\",\"81114021050000274544001052\",\"02175000090000000009545441\",\"11114021050000274544001051\",\"33116022020000000218870137\",\"06175000090000000023513951\"],\"residenceAddress\":null,\"vatStatus\":\"Czynny\"}}";
                JObject json = JObject.Parse(data);
                InvoiceName = json["data"]["name"].ToString();
                InvoiceCity = json["data"]["city"].ToString();
                InvoicePostCode = json["data"]["postalCode"].ToString();
                InvoiceStreet = json["data"]["street"].ToString();
                InvoiceStreetNum = json["data"]["buildingNumber"].ToString();
                InvoiceLocalNum = "";
                ApiLoading = false;
            }
            else
            {
                UserDialogs.Instance.Toast("Wprowadzony NIP jest nieprawidłowy.");
            }
        }
    }
}
