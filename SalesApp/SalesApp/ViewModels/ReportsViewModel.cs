using Acr.UserDialogs;
using Microcharts;
using SalesApp.Effects;
using SalesApp.Fiscal;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Packets packets;
        private TabbedPage mainPage;
        private string year;
        private string typeYear;
        private string month;
        private bool openYearPrompt;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command GoBackCommand { get; set; }
        public Command PrintDailyCommand { get; set; }
        public Command PrintMonthlyCommand { get; set; }
        public Command PrintPeriodicalCommand { get; set; }

        public Command CurrentPageChangedCommand { get; set; }
        private string _SumValue;
        private string _SumMonthlyValue;
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
        public string SumMonthlyValue
        {
            get
            {
                return _SumMonthlyValue;
            }
            set
            {
                if (_SumMonthlyValue != value)
                {
                    _SumMonthlyValue = value;
                    OnPropertyChanged("SumMonthlyValue");
                }
            }
        }
        private Chart _MyBarChart;
        public Chart MyBarChart
        {
            get
            {
                return _MyBarChart;
            }
            set
            {
                if (_MyBarChart != value)
                {
                    _MyBarChart = value;
                    OnPropertyChanged("MyBarChart");
                }
            }
        }
        private Chart _MyMonthlyBarChart;
        public Chart MyMonthlyBarChart
        {
            get
            {
                return _MyMonthlyBarChart;
            }
            set
            {
                if (_MyMonthlyBarChart != value)
                {
                    _MyMonthlyBarChart = value;
                    OnPropertyChanged("MyMonthlyBarChart");
                }
            }
        }

        private List<string> _MonthList;
        public List<string> MonthList
        {
            get
            {
                return _MonthList;
            }
            set
            {
                if (_MonthList != value)
                {
                    _MonthList = value;
                    OnPropertyChanged("MonthList");
                }
            }
        }
        private int _SelectedMonth;
        public int SelectedMonth
        {
            get
            {
                return _SelectedMonth;
            }
            set
            {
                if (_SelectedMonth != value)
                {
                    _SelectedMonth = value;
                    pickMonthAndYear();
                    openYearPrompt = true;
                }
            }
        }
        private int _MonthlyWidth;
        public int MonthlyWidth
        {
            get
            {
                return _MonthlyWidth;
            }
            set
            {
                if (_MonthlyWidth != value)
                {
                    _MonthlyWidth = value;
                    OnPropertyChanged("MonthlyWidth");
                }
            }
        }
        private int _DailyWidth;
        public int DailyWidth
        {
            get
            {
                return _DailyWidth;
            }
            set
            {
                if (_DailyWidth != value)
                {
                    _DailyWidth = value;
                    OnPropertyChanged("DailyWidth");
                }
            }
        }

        private string _PeroidSum;
        public string PeroidSum
        {
            get
            {
                return _PeroidSum;
            }
            set
            {
                if (_PeroidSum != value)
                {
                    _PeroidSum = value;
                    OnPropertyChanged("PeroidSum");
                }
            }
        }
        private string _PeroidReceipts;
        public string PeroidReceipts
        {
            get
            {
                return _PeroidReceipts;
            }
            set
            {
                if (_PeroidReceipts != value)
                {
                    _PeroidReceipts = value;
                    OnPropertyChanged("PeroidReceipts");
                }
            }
        }

        private DateTime _FromDate;
        public DateTime FromDate
        {
            get
            {
                return _FromDate;
            }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                    if(!initState)
                        ReadSalesToPeroidReport();
                }
            }
        }

        private DateTime _ToDate;
        public DateTime ToDate
        {
            get
            {
                return _ToDate;
            }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                    if(!initState)
                        ReadSalesToPeroidReport();
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

        private List<Sales> SalesFormDb;       
        private List<Sales> MonthlySalesFromDb;
        private List<Sales> PeroidSalesFormDb;
        private string chartColor = "#2196f1";
        private float sum = 0;
        private float monthlySum = 0;
        private float peroidSum = 0;
        private bool initState = true;
        private DateTime dateTime;

        public ReportsViewModel(TabbedPage main)
        {
            SetTheme();
            mainPage = main;
            openYearPrompt = false;
            packets = new Packets();
            dateTime = DateTime.Now;
            year = dateTime.Year.ToString();
            MonthList = new List<string>{ "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
            ReadSalesAndSetChart();           
            SetSelectedMonth(dateTime);   
            ReadSalesAndSetMonthlyChart(dateTime.ToString("dd.MM.yyyy").Substring(3));
            GoBackCommand = new Command(async () => await GoBack());
            PrintDailyCommand = new Command(async () => await PrintDailyReport());
            PrintMonthlyCommand = new Command(async () => await PrintMonthlyReport());
            PrintPeriodicalCommand = new Command(async () => await PrintPeriodicalReport());
            CurrentPageChangedCommand = new Command(async () => await CurrentPageChanged());
            ToDate = dateTime;
            FromDate = dateTime;
            ReadSalesToPeroidReport();
            initState = false;
        }
        private void SetTheme()
        { 
            if (Theme.IsDarkMode)
            {
                BackgroundColor = DarkTheme.BackgroundColor;
                LabelTextColor = DarkTheme.LabelTextColor;
                FieldsTextColor = DarkTheme.FieldsTextColor;
                FieldsBorderColor = DarkTheme.FieldsBorderColor;
            }
            else
            {
                BackgroundColor = LightTheme.BackgroundColor;
                LabelTextColor = LightTheme.LabelTextColor;
                FieldsTextColor = LightTheme.FieldsTextColor;
                FieldsBorderColor = LightTheme.FieldsBorderColor;
            }
        }
        private Task CurrentPageChanged()
        {
            throw new NotImplementedException();
        }

        private async void ReadSalesToPeroidReport()
        {
            peroidSum = 0;
            
            PeroidSalesFormDb = await App.SQLiteDb.ReadAllSalesBetweenDates(FromDate, ToDate);
            foreach(var item in PeroidSalesFormDb)
            {
                peroidSum += item.Value;
            }
            PeroidSum = $"Suma: {peroidSum}";
            PeroidReceipts = $"Sprzedaży: {PeroidSalesFormDb.Count}";
        }

        private async void ReadSalesAndSetChart()
        {
            SalesFormDb = await App.SQLiteDb.ReadAllSalesByDate(dateTime.ToString("dd.MM.yyyy"));
            var GroupedSales = SalesFormDb.GroupBy(x => x.ProductName).Select(x => new Sales 
                { 
                    ProductName = x.Key,
                    Value = x.Sum(y => y.Value)
                }).ToList();
            List<ChartEntry> DataList = new List<ChartEntry>();
            foreach (var product in GroupedSales)
            {
                DataList.Add(new ChartEntry(product.Value)
                {
                    Label = product.ProductName,
                    ValueLabel = product.Value.ToString(),
                    Color = SkiaSharp.SKColor.Parse(chartColor),
                    ValueLabelColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#ffffff" : "#000000")
                });
                ChangeColor();   
                sum += product.Value;
            }
            DailyWidth = DataList.Count * 30;
            MyBarChart = new BarChart() { Entries = DataList, LabelTextSize = 30, BackgroundColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#202124" : "#fafafa"), LabelColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#ffffff" : "#000000"), };
            SetSumValue();
        }
        private async void ReadSalesAndSetMonthlyChart(string month)
        {           
            MonthlySalesFromDb = await App.SQLiteDb.ReadAllSalesByMonthAndYear(month);
            var GroupedSales = MonthlySalesFromDb.GroupBy(x => x.ProductName).Select(x => new Sales
            {
                ProductName = x.Key,
                Value = x.Sum(y => y.Value)
            }).ToList();
            List<ChartEntry> DataList = new List<ChartEntry>();
            monthlySum = 0;
            foreach (var product in GroupedSales)
            {            
                DataList.Add(new ChartEntry(product.Value)
                {
                    Label = product.ProductName,
                    ValueLabel = product.Value.ToString(),
                    Color = SkiaSharp.SKColor.Parse(chartColor),
                    ValueLabelColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#ffffff" : "#000000")
                });
                ChangeColor();
                monthlySum += product.Value;
            }
            MonthlyWidth = DataList.Count * 30;
            MyMonthlyBarChart = new BarChart() { Entries = DataList, LabelTextSize = 30, BackgroundColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#202124" : "#fafafa"), LabelColor = SkiaSharp.SKColor.Parse(Theme.IsDarkMode ? "#ffffff" : "#000000") };
            SetMonthlySum();
        }
        private Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void ChangeColor()
        {
            chartColor = (chartColor == "#2196f1") ? (Theme.IsDarkMode ? "#ededed" :"#858585") : "#2196f1";
           
        }
        private void SetSumValue()
        {
            SumValue = $"Suma: {sum}";
        }
        private void SetMonthlySum()
        {
            SumMonthlyValue = $"Suma: {monthlySum}";
        }
        private void SetSelectedMonth(DateTime date)
        {
            string month = date.ToString().Substring(3, 2);
            SelectedMonth = SetMonthIndex(month);       
        }

        private async Task PrintDailyReport()
        {
            DateTime dateTime = DateTime.Now;

            if (TCPConnection.SocketConnected())
            {
                if (await UserDialogs.Instance.ConfirmAsync($"{dateTime} \nCzy data jest aktualna?", "Wydrukować raport dobowy?", "Tak", "Nie"))
                {
                    TCPConnection.Send(packets.DailyReport(dateTime));
                    TimeSpan timeSpan = TimeSpan.FromSeconds(5);
                    UserDialogs.Instance.Toast("Drukowanie raportu...", timeSpan);
                    
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Brak połączenia z drukarką!");
            }
        }

        private async Task PrintMonthlyReport()
        {
            if (TCPConnection.SocketConnected())
            {
                string monthPickString = month.ToString();
                string monthNowString = dateTime.Month.ToString();
                string monthPick = (monthPickString.Length == 1) ? "0" + monthPickString : monthPickString;
                string monthNow = (monthNowString.Length == 1) ? "0" + monthNowString : monthNowString;

                int validationTokenPicked = int.Parse(typeYear.ToString() + monthPick);
                int validationTokenNow = int.Parse(dateTime.Year.ToString() + monthNow);

                if (month.Equals(dateTime.Month.ToString()) || validationTokenPicked >= validationTokenNow)
                {
                    UserDialogs.Instance.Toast("Nie można wydrukować raportu z niezakończonego miesiąca!");
                }
                else
                {
                    var config = new ActionSheetConfig
                    {
                        Cancel = new ActionSheetOption("Anuluj"),
                        Title = "Typ raportu:"
                    };

                    config.Add("Fiskalny skrócony", new Action(async () =>
                    {
                        TCPConnection.Send(packets.MonthlyReport(false, dateTime.Day.ToString(), month, typeYear));
                        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                        UserDialogs.Instance.Toast("Drukowanie raportu...", timeSpan);
                    }));

                    config.Add("Fiskalny pełny", new Action(async () =>
                    {
                        TCPConnection.Send(packets.MonthlyReport(true, dateTime.Day.ToString(), month, typeYear));
                        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                        UserDialogs.Instance.Toast("Drukowanie raportu...", timeSpan);
                    }));
                    UserDialogs.Instance.ActionSheet(config);
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Brak połączenia z drukarką!");
            }

        }

        private async Task PrintPeriodicalReport()
        {
            if (TCPConnection.SocketConnected())
            {
                if(PeroidSalesFormDb.Count == 0)
                {
                    UserDialogs.Instance.Toast("Nie można wydrukować raportu z zakresu, w którym nie odnotowano sprzedaży!");
                }
                else if (ToDate < FromDate || ToDate > DateTime.Now)
                {
                    UserDialogs.Instance.Toast("Nie można wydrukować raportu z błędnego zakresu!");
                }
                else
                {
                    var config = new ActionSheetConfig
                    {
                        Cancel = new ActionSheetOption("Anuluj"),
                        Title = "Typ raportu:"
                    };

                    config.Add("Fiskalny skrócony", new Action(async () =>
                    {
                        TCPConnection.Send(packets.PeroidReport(false, FromDate, ToDate));
                        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                        UserDialogs.Instance.Toast("Drukowanie raportu...", timeSpan);
                    }));

                    config.Add("Fiskalny pełny", new Action(async () =>
                    {
                        TCPConnection.Send(packets.PeroidReport(true, FromDate, ToDate));
                        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
                        UserDialogs.Instance.Toast("Drukowanie raportu...", timeSpan);
                    }));
                    UserDialogs.Instance.ActionSheet(config);
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Brak połączenia z drukarką!");
            }

        }

        private async Task getYear(bool open)
        {
            if (open)
            {
                var input = await mainPage.DisplayPromptAsync("Podaj rok:", "", "Potwierdź", "", "", -1, Keyboard.Numeric, year);
                typeYear = input.ToString();
            }
            else
            {
                typeYear = year;
            }
        }


        private async void pickMonthAndYear()
        {
            await getYear(openYearPrompt);
            ReadSalesAndSetMonthlyChart(SetMonthToQuery(typeYear));
            OnPropertyChanged("SelectedMonth");
        }
        private int SetMonthIndex(string month)
        {
            switch (month)
            {
                case "01":
                    return 0;
                case "02":
                    return 1;
                case "03":
                    return 2;
                case "04":
                    return 3;
                case "05":
                    return 4;
                case "06":
                    return 5;
                case "07":
                    return 6;
                case "08":
                    return 7;
                case "09":
                    return 8;
                case "10":
                    return 9;
                case "11":
                    return 10;
                case "12":
                    return 11;
                default:
                    return 0;
            }
        }
        private string SetMonthToQuery(string year)
        {
            switch (SelectedMonth)
            {
                case 0:
                    month = "1";
                    return "01." + year;
                case 1:
                    month = "2";
                    return "02." + year; ;
                case 2:
                    month = "3";
                    return "03." + year; ;
                case 3:
                    month = "4";
                    return "04." + year; ;
                case 4:
                    month = "5";
                    return "05." + year; ;
                case 5:
                    month = "6";
                    return "06." + year; ;
                case 6:
                    month = "7";
                    return "07." + year; ;
                case 7:
                    month = "8";
                    return "08." + year; ;
                case 8:
                    month = "9";
                    return "09." + year; ;
                case 9:
                    month = "10";
                    return "10." + year; ;
                case 10:
                    month = "12";
                    return "11." + year; ;
                case 11:
                    month = "12";
                    return "12." + year; ;
                default:
                    month = "1";
                    return "01." + year; ;
            }
        }
    }
}
