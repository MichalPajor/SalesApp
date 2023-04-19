using Acr.UserDialogs;
using SalesApp.Effects;
using SalesApp.Fiscal;
using SalesApp.Helpers;
using SalesApp.Models;
using SalesApp.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<MainMenuItem> _MySource;
        public ObservableCollection<MainMenuItem> MySource
        {
            get
            {
                return _MySource;
            }
            set
            {
                if (_MySource != value)
                {
                    _MySource = value;
                    OnPropertyChanged("MySource");
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

        public Command NavigationCommand { get; set; }
        //public ObservableCollection<MainMenuItem> MySource { get; set; }
        private Packets packets;
        public MainMenuViewModel()
        {
            SetTheme();
            packets = new Packets();
            MySource = new ObservableCollection<MainMenuItem>() {

            new MainMenuItem(){Id = 0, ItemTitle ="Raport dobowy" ,BgImageSource= Theme.IsDarkMode? "@drawable/daily_rep_dark_theme.png":"@drawable/daily_rep.png", BorderColor = Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor,},

            new MainMenuItem(){Id = 1, ItemTitle ="Raporty" ,BgImageSource= Theme.IsDarkMode? "@drawable/sales_rep_dark_theme.png":"@drawable/sales_rep.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 2, ItemTitle ="Baza towarowa" ,BgImageSource= Theme.IsDarkMode? "@drawable/database_dark_theme.png":"@drawable/database.png", BorderColor = Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 3, ItemTitle ="Stawki VAT" ,BgImageSource= Theme.IsDarkMode? "@drawable/tax_dark_theme.png":"@drawable/tax.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 4, ItemTitle ="Jednostki miary" ,BgImageSource= Theme.IsDarkMode? "@drawable/measure_dark_theme.png":"@drawable/measure.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 5, ItemTitle ="Sprzedaż" ,BgImageSource= "@drawable/sale.png", BorderColor=LightTheme.AccentColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = LightTheme.AccentColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 6, ItemTitle ="Motyw" ,BgImageSource= Theme.IsDarkMode? "@drawable/change_theme_dark_theme.png" : "@drawable/change_theme.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 7, ItemTitle ="Zamknij" ,BgImageSource= Theme.IsDarkMode? "@drawable/exit_dark_theme.png":"@drawable/exit.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 8, ItemTitle ="Nabywcy" ,BgImageSource= Theme.IsDarkMode? "@drawable/contractor_dark_theme.png":"@drawable/contractor.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 9, ItemTitle ="Połączenie" ,BgImageSource= Theme.IsDarkMode? "@drawable/connect_dark_theme.png":"@drawable/connect.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 10, ItemTitle ="O programie" ,BgImageSource= Theme.IsDarkMode? "@drawable/info_dark_theme.png":"@drawable/info.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 11, ItemTitle ="Licencje" ,BgImageSource= Theme.IsDarkMode? "@drawable/license_dark_theme.png":"@drawable/license.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},
            };
            NavigationCommand = new Command<int>(
           execute: async (id) =>
           {
               switch (id)
               {
                   case 0:
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
                       break;
                   case 1:
                       {
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ReportsPage()));
                       }
                       break;
                   case 2:
                       {
                           ViewModelLocator._GoodsViewModel = new GoodsViewModel();
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new GoodsPage()));
                       }
                       break;
                   case 3:
                       {
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new TaxRatesPage()));
                       }
                       break;
                   case 4:
                       {
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new MeasuresPage()));
                       }
                       break;
                   case 5:
                       {
                           ViewModelLocator._SaleViewModel = new SaleViewModel();
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new SalePage()));
                       }
                       break;
                  
                   case 6:
                       {
                           ChangeTheme();
                       }
                       break;
                   case 7:
                       {
                           if(await UserDialogs.Instance.ConfirmAsync("Czy zamknąć aplikację?", "Zamknij", "Tak", "Nie")){
                               System.Diagnostics.Process.GetCurrentProcess().Kill();
                           }
                       }
                       break;
                   case 8:
                       {
                           ViewModelLocator._ContractorsViewModel = new ContractorsViewModel();
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ContractorsPage()));
                       }
                       break;
                   case 9:
                       {
                           var config = new ActionSheetConfig
                           {
                               Cancel = new ActionSheetOption("Anuluj"),
                               Title = "Połączenie"
                           };
                           config.Add("Odśwież", new Action(async () =>
                           {
                               if (TCPConnection.SocketConnected())
                               {
                                   UserDialogs.Instance.Toast("Połączenie jest już aktywne :)");
                               }
                               else
                               {
                                   var ipAddressSetting = await App.SQLiteDb.ReadAppSetting("IPAddress");
                                   var portSetting = await App.SQLiteDb.ReadAppSetting("Port");
                                   var isOnDevice = await App.SQLiteDb.ReadAppSetting("IsOnDevice");
                                   IPAddress ipAddress;
                                   UserDialogs.Instance.Toast("Łączenie...");
                                   try
                                   {
                                       ipAddress = Convert.ToBoolean(isOnDevice.Value)? System.Net.IPAddress.Parse("127.0.0.1") : System.Net.IPAddress.Parse(ipAddressSetting.Value);
                                   }
                                   catch
                                   {
                                       UserDialogs.Instance.Toast("Wpisano niepoprawny adres IP");
                                       return;
                                   }

                                   await Task.Run(() => TCPConnection.StartTCPClient(ipAddress, Convert.ToBoolean(isOnDevice.Value) ? 6001 : int.Parse(portSetting.Value)));
                                   await Task.Delay(500);

                                   if (TCPConnection.SocketConnected())
                                   {
                                       UserDialogs.Instance.Toast("Połączono poprawnie.");
                                   }
                                   else
                                   {
                                       UserDialogs.Instance.Toast("Sprawdź poprawność połączenia i spróbuj ponownie.");
                                   }
                               }
                           }));
                           config.Add("Edytuj", new Action(async () =>
                           {
                               await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConnectEditPage()));
                           }));
                           UserDialogs.Instance.ActionSheet(config);
                          
                              
                       }
                       break;
                   case 10:
                       {
                           await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AboutPage()));
                       }
                       break;
                   case 11:
                       {
                           await UserDialogs.Instance.ConfirmAsync("Ikony: flaticon.com/free-icons \n\nLicense icons created by cenalsi" +
                               "\n\nReport, Tax, Exit, WiFi, About, Payment icons created by Freepik\n\nReport, Database icons created by Pixel perfect\n\nPayment method icons created by xnimrodx" +
                               "\n\nContrast icons created by Smartline\n\nMembers icons created by Bombasticon Studio", "Licencje:", "Ok", "Anuluj");
                       }
                       break;
               }
           },
           canExecute: (id) =>
           {
               return true;
           });

        }
        private async void SetTheme()
        {
            var theme = await App.SQLiteDb.ReadAppSetting("DarkTheme");
            if (Theme.IsDarkMode)
            {
                BackgroundColor = DarkTheme.BackgroundColor;
                theme.Value = "true";
            }
            else
            {
                BackgroundColor = LightTheme.WhiteColor;
                theme.Value = "false";
            }
            await App.SQLiteDb.UpdateAppSetting(theme);
        }
        private void ChangeTheme()
        {
            Theme.IsDarkMode = !Theme.IsDarkMode;
            MySource = new ObservableCollection<MainMenuItem>() {

            new MainMenuItem(){Id = 0, ItemTitle ="Raport dobowy" ,BgImageSource= Theme.IsDarkMode? "@drawable/daily_rep_dark_theme.png":"@drawable/daily_rep.png", BorderColor = Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor,},

            new MainMenuItem(){Id = 1, ItemTitle ="Raporty" ,BgImageSource= Theme.IsDarkMode? "@drawable/sales_rep_dark_theme.png":"@drawable/sales_rep.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 2, ItemTitle ="Baza towarowa" ,BgImageSource= Theme.IsDarkMode? "@drawable/database_dark_theme.png":"@drawable/database.png", BorderColor = Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 3, ItemTitle ="Stawki VAT" ,BgImageSource= Theme.IsDarkMode? "@drawable/tax_dark_theme.png":"@drawable/tax.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 4, ItemTitle ="Jednostki miary" ,BgImageSource= Theme.IsDarkMode? "@drawable/measure_dark_theme.png":"@drawable/measure.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 5, ItemTitle ="Sprzedaż" ,BgImageSource= "@drawable/sale.png", BorderColor=LightTheme.AccentColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = LightTheme.AccentColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 6, ItemTitle ="Motyw" ,BgImageSource= Theme.IsDarkMode? "@drawable/change_theme_dark_theme.png" : "@drawable/change_theme.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 7, ItemTitle ="Zamknij" ,BgImageSource= Theme.IsDarkMode? "@drawable/exit_dark_theme.png":"@drawable/exit.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 8, ItemTitle ="Nabywcy" ,BgImageSource= Theme.IsDarkMode? "@drawable/contractor_dark_theme.png":"@drawable/contractor.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 9, ItemTitle ="Połączenie" ,BgImageSource= Theme.IsDarkMode? "@drawable/connect_dark_theme.png":"@drawable/connect.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 10, ItemTitle ="O programie" ,BgImageSource= Theme.IsDarkMode? "@drawable/info_dark_theme.png":"@drawable/info.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},

            new MainMenuItem(){Id = 11, ItemTitle ="Licencje" ,BgImageSource= Theme.IsDarkMode? "@drawable/license_dark_theme.png":"@drawable/license.png", BorderColor=Theme.IsDarkMode? DarkTheme.LabelTextColor : LightTheme.FieldsTextColor,
                BackColor = Theme.IsDarkMode? DarkTheme.BackgroundColor : LightTheme.BackgroundColor, TextColor = Theme.IsDarkMode? DarkTheme.FieldsTextColor : LightTheme.FieldsTextColor,
            BackIconsColor = Theme.IsDarkMode? DarkTheme.IconsBackColor : LightTheme.WhiteColor},
            };
            SetTheme();
        }
    }
}
