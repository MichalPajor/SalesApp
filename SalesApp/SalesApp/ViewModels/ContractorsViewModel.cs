using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using SalesApp.Effects;
using SalesApp.Helpers;
using SalesApp.Models;
using SalesApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class ContractorsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Command SaveCommand { get; set; }
        public Command GoBackCommand { get; set; }
        public Command GoBackFromEditCommand { get; set; }
        public Command UpdateCommand { get; set; }     
        public Command GetInfoFromAPI { get; set; }


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

        private Contractor editedContractor;

        public ICommand ItemClickCommand
        {
            get
            {
                return new Command(async (item) =>
                {
                    var clicked = item as Contractor;
                    editedContractor = await App.SQLiteDb.ReadContractorByNIP(clicked.NIP);
                    var config = new ActionSheetConfig
                    {
                        Cancel = new ActionSheetOption("Anuluj"),
                        Title = $"{clicked.Name} - opcje"
                    };

                    config.Add("Usuń", new Action(async () =>
                    {
                        if (await UserDialogs.Instance.ConfirmAsync($"Czy na pewno usunąć {clicked.Name}? \n", "Usuń", "Tak", "Nie"))
                        {
                            await App.SQLiteDb.DeleteContractor(clicked);
                            ContractorsList.Remove(clicked);
                            UserDialogs.Instance.Toast("Usunięto");
                        }
                    }));

                    config.Add("Edytuj", new Action(async () =>
                    {
                        InvoiceNIP = clicked.NIP;
                        InvoiceName = clicked.Name;
                        InvoiceStreet = clicked.Street;
                        InvoiceLocalNum = clicked.LocalNum;
                        InvoicePostCode = clicked.PostCode;
                        InvoiceStreetNum = clicked.StreetNum;
                        InvoiceCity = clicked.City;
                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ContractorsEditPage()));

                    }));

                    UserDialogs.Instance.ActionSheet(config);

                });
            }
        }

        public ContractorsViewModel()
        {
            SetTheme();
            ReadAllContractors();
            SaveCommand = new Command(async () => await SaveContractor());
            UpdateCommand = new Command(async () => await UpdateContractor());
            GoBackCommand = new Command(async () => await GoBack());
            GoBackFromEditCommand = new Command(async () => await GoBackFromEdit());
            GetInfoFromAPI = new Command(async () => await GetApiInfo());
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

        private async Task UpdateContractor()
        {
            if (String.IsNullOrEmpty(InvoiceNIP) || String.IsNullOrEmpty(InvoiceCity) || String.IsNullOrEmpty(InvoiceStreet) || String.IsNullOrEmpty(InvoiceStreetNum) || String.IsNullOrEmpty(InvoicePostCode))
            {
                UserDialogs.Instance.Toast("Uzupełnij wszystkie wymagane pola.");
            } else if (!ValidateNIP.IsValidNIP(InvoiceNIP))
            {
                UserDialogs.Instance.Toast("Wprowadzony NIP jest nieprawidłowy.");
            }
            else
            {
                var contractorFromDb = await App.SQLiteDb.ReadContractorByNIP(InvoiceNIP);
                if (contractorFromDb is null)
                {
                    UserDialogs.Instance.Toast("Nabywca o tym numerze NIP nie istnieje w bazie.");
                }
                else
                {
                    contractorFromDb.Name = InvoiceName;
                    contractorFromDb.NIP = InvoiceNIP;
                    contractorFromDb.City = InvoiceCity;
                    contractorFromDb.Street = InvoiceStreet;
                    contractorFromDb.StreetNum = InvoiceStreetNum;
                    contractorFromDb.LocalNum = String.IsNullOrEmpty(InvoiceLocalNum)? "" : InvoiceLocalNum;
                    contractorFromDb.PostCode = InvoicePostCode;

                    await App.SQLiteDb.UpdateContractor(contractorFromDb);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    UserDialogs.Instance.Toast("Edytowano pomyślnie");
                    ReadAllContractors();
                    ClearContractorsFields();
                }
            }
        }

        private async Task SaveContractor()
        {
            if (String.IsNullOrEmpty(InvoiceNIP) || String.IsNullOrEmpty(InvoiceCity) || String.IsNullOrEmpty(InvoiceStreet) || String.IsNullOrEmpty(InvoiceStreetNum) || String.IsNullOrEmpty(InvoicePostCode))
            {
                UserDialogs.Instance.Toast("Uzupełnij wszystkie pola.");
            }
            else if (!ValidateNIP.IsValidNIP(InvoiceNIP))
            {
                UserDialogs.Instance.Toast("Wprowadzony NIP jest nieprawidłowy.");
            }
            else
            {               
                var contractorFromDb = await App.SQLiteDb.ReadContractorByNIP(InvoiceNIP);
                if(contractorFromDb != null)
                {
                    UserDialogs.Instance.Toast("Nabywca o tym numerze NIP już istnieje.");
                }
                else
                {
                    Contractor contractor = new Contractor()
                    {
                        Name = InvoiceName,
                        NIP = InvoiceNIP,
                        City = InvoiceCity,
                        Street = InvoiceStreet,
                        StreetNum = InvoiceStreetNum,
                        LocalNum = String.IsNullOrEmpty(InvoiceLocalNum) ? "" : InvoiceLocalNum,
                        PostCode = InvoicePostCode
                    };
                    await App.SQLiteDb.InsertContractor(contractor);
                    UserDialogs.Instance.Toast("Zapisano pomyślnie");
                    ContractorsList.Add(contractor);
                    ClearContractorsFields();

                }
            }
        }
        private Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void ReadAllContractors()
        {
            ContractorsList = new ObservableCollection<Contractor>(await App.SQLiteDb.ReadAllContractors());
        }
        private Task GoBackFromEdit()
        {
            ClearContractorsFields();
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void ClearContractorsFields()
        {
            InvoiceNIP = "";
            InvoiceName = "";
            InvoiceStreet = "";
            InvoiceLocalNum = "";
            InvoicePostCode = "";
            InvoiceStreetNum = "";
            InvoiceCity = "";
        }
    }
}
