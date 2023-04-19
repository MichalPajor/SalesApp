using Acr.UserDialogs;
using SalesApp.Effects;
using SalesApp.Fiscal;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class TaxRatesViewModel : INotifyPropertyChanged
    {
        private Packets packets;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command SetTaxRatesCommand { get; set; }

        private List<TaxRates> taxRatesDB;

        private string _ValueA;
        private bool _FreeA;
        private string _ValueB;
        private bool _FreeB;
        private string _ValueC;
        private bool _FreeC;
        private string _ValueD;
        private bool _FreeD;
        private string _ValueE;
        private bool _FreeE;
        private string _ValueF;
        private bool _FreeF;
        private string _ValueG;
        private bool _FreeG;

        public string ValueA
        {
            get
            {
                return _ValueA;
            }
            set
            {
                if (_ValueA != value)
                {
                    _ValueA = value;
                    OnPropertyChanged("ValueA");
                }
            }
        }      
        public bool FreeA
        {
            get
            {
                return _FreeA;
            }
            set
            {
                if (_FreeA != value)
                {
                    _FreeA = value;
                    OnPropertyChanged("FreeA");
                }
            }
        }
        public string ValueB
        {
            get
            {
                return _ValueB;
            }
            set
            {
                if (_ValueB != value)
                {
                    _ValueB = value;
                    OnPropertyChanged("ValueB");
                }
            }
        }
        public bool FreeB
        {
            get
            {
                return _FreeB;
            }
            set
            {
                if (_FreeB != value)
                {
                    _FreeB = value;
                    OnPropertyChanged("FreeB");
                }
            }
        }
        public string ValueC
        {
            get
            {
                return _ValueC;
            }
            set
            {
                if (_ValueC != value)
                {
                    _ValueC = value;
                    OnPropertyChanged("ValueC");
                }
            }
        }
        public bool FreeC
        {
            get
            {
                return _FreeC;
            }
            set
            {
                if (_FreeC != value)
                {
                    _FreeC = value;
                    OnPropertyChanged("FreeC");
                }
            }
        }
        public string ValueD
        {
            get
            {
                return _ValueD;
            }
            set
            {
                if (_ValueD != value)
                {
                    _ValueD = value;
                    OnPropertyChanged("ValueD");
                }
            }
        }
        public bool FreeD
        {
            get
            {
                return _FreeD;
            }
            set
            {
                if (_FreeD != value)
                {
                    _FreeD = value;
                    OnPropertyChanged("FreeD");
                }
            }
        }
        public string ValueE
        {
            get
            {
                return _ValueE;
            }
            set
            {
                if (_ValueE != value)
                {
                    _ValueE = value;
                    OnPropertyChanged("ValueE");
                }
            }
        }
        public bool FreeE
        {
            get
            {
                return _FreeE;
            }
            set
            {
                if (_FreeE != value)
                {
                    _FreeE = value;
                    OnPropertyChanged("FreeE");
                }
            }
        }
        public string ValueF
        {
            get
            {
                return _ValueF;
            }
            set
            {
                if (_ValueF != value)
                {
                    _ValueF = value;
                    OnPropertyChanged("ValueF");
                }
            }
        }
        public bool FreeF
        {
            get
            {
                return _FreeF;
            }
            set
            {
                if (_FreeF != value)
                {
                    _FreeF = value;
                    OnPropertyChanged("FreeF");
                }
            }
        }
        public string ValueG
        {
            get
            {
                return _ValueG;
            }
            set
            {
                if (_ValueG != value)
                {
                    _ValueG = value;
                    OnPropertyChanged("ValueG");
                }
            }
        }
        public bool FreeG
        {
            get
            {
                return _FreeG;
            }
            set
            {
                if (_FreeG != value)
                {
                    _FreeG = value;
                    OnPropertyChanged("FreeG");
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
        public TaxRatesViewModel()
        {
            SetTheme();
            packets = new Packets();
            taxRatesDB = new List<TaxRates>();
            ReadAllTaxRates();
            GoBackCommand = new Command(async () => await GoBack());
            SetTaxRatesCommand = new Command(async () => await SetTaxRates());
            SaveCommand = new Command(
            execute: async () =>
            {
                if (taxRatesDB.Count == 0) //init values
                {
                    await Save(ValueA, "A", FreeA);
                    await Save(ValueB, "B", FreeB);
                    await Save(ValueC, "C", FreeC);
                    await Save(ValueD, "D", FreeD);
                    await Save(ValueE, "E", FreeE);
                    await Save(ValueF, "F", FreeF);
                    await Save(ValueG, "G", FreeG);
                }
                else //update values
                {
                    await Update(ValueA, "A", FreeA);
                    await Update(ValueB, "B", FreeB);
                    await Update(ValueC, "C", FreeC);
                    await Update(ValueD, "D", FreeD);
                    await Update(ValueE, "E", FreeE);
                    await Update(ValueF, "F", FreeF);
                    await Update(ValueG, "G", FreeG);
                }
                UserDialogs.Instance.Toast("Zapisano pomyślnie");
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
        private async Task SetTaxRates()
        {
            if (TCPConnection.SocketConnected())
            {
                var taxRatesDB = await App.SQLiteDb.ReadAllTaxRates();
                TCPConnection.Send(packets.SetTaxRates(taxRatesDB[0].Value, taxRatesDB[1].Value, taxRatesDB[2].Value, taxRatesDB[3].Value, taxRatesDB[4].Value, taxRatesDB[5].Value, taxRatesDB[6].Value));
                TimeSpan timeSpan = TimeSpan.FromSeconds(5);
                UserDialogs.Instance.Toast("Potwierdź zmianę na urządzeniu...", timeSpan);
            }
            else
            {
                UserDialogs.Instance.Toast("Brak połączenia z drukarką!");
            }
        }

        private Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task<int> Save(string value, string name, bool isFree)
        {
            TaxRates tax = new TaxRates();
            if (isFree)
            {
                tax.Value = 98.99;
            }
            else if (value is null || value.Equals(""))
            {
                tax.Value = 99.99;
            }
            else
            {
                tax.Value = double.Parse(value);
            }
            tax.Name = name;
            return await App.SQLiteDb.InsertTaxRate(tax);
        }

        private async void ReadAllTaxRates()
        {
            taxRatesDB = await App.SQLiteDb.ReadAllTaxRates();
            foreach(var item in taxRatesDB)
            {
                switch (item.Name)
                {
                    case "A":
                        if(item.Value == 98.99)
                        {
                            FreeA = true;
                        }
                        else if(item.Value != 99.99)
                        {
                            ValueA = item.Value.ToString();
                        }
                        break;
                    case "B":
                        if (item.Value == 98.99)
                        {
                            FreeB = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueB = item.Value.ToString();
                        }
                        break;
                    case "C":
                        if (item.Value == 98.99)
                        {
                            FreeC = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueC = item.Value.ToString();
                        }
                        break;
                    case "D":
                        if (item.Value == 98.99)
                        {
                            FreeD = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueD = item.Value.ToString();
                        }
                        break;
                    case "E":
                        if (item.Value == 98.99)
                        {
                            FreeE = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueE = item.Value.ToString();
                        }
                        break;
                    case "F":
                        if (item.Value == 98.99)
                        {
                            FreeF = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueF = item.Value.ToString();
                        }
                        break;
                    case "G":
                        if (item.Value == 98.99)
                        {
                            FreeG = true;
                        }
                        else if (item.Value != 99.99)
                        {
                            ValueG = item.Value.ToString();
                        }
                        break;
                }
            }
        }

        private async Task<int> Update(string value, string name, bool isFree)
        {
            TaxRates tax = new TaxRates();
            if (isFree)
            {
                tax.Value = 98.99;
            }
            else if (value is null || value.Equals(""))
            {
                tax.Value = 99.99;
            }
            else
            {
                tax.Value = double.Parse(value);
            }
            tax.Name = name;
            switch (name)
            {
                case "A":
                    tax.Id = 1;
                    break;
                case "B":
                    tax.Id = 2;
                    break;
                case "C":
                    tax.Id = 3;
                    break;
                case "D":
                    tax.Id = 4;
                    break;
                case "E":
                    tax.Id = 5;
                    break;
                case "F":
                    tax.Id = 6;
                    break;
                case "G":
                    tax.Id = 7;
                    break;
            }
            return await App.SQLiteDb.UpdateTaxRate(tax);
        }
    }
}
