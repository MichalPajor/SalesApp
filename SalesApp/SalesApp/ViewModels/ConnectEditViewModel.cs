using Acr.UserDialogs;
using SalesApp.Effects;
using SalesApp.Fiscal;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class ConnectEditViewModel : INotifyPropertyChanged
    {
        public Command GoBackCommand { get; set; }
        public Command ConnectCommand { get; set; }

        private string _IPAddress;
        private string _Port;
        private bool _IsAutoConnect;
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                if (_IPAddress != value)
                {
                    _IPAddress = value;
                    OnPropertyChanged("IPAddress");
                }
            }
        }
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                if (_Port != value)
                {
                    _Port = value;
                    OnPropertyChanged("Port");
                }
            }
        }
        public bool IsAutoConnect
        {
            get
            {
                return _IsAutoConnect;
            }
            set
            {
                if (_IsAutoConnect != value)
                {
                    _IsAutoConnect = value;
                    OnPropertyChanged("IsAutoConnect");
                }
            }
        }
        private bool _IPEntryEnable;
        public bool IPEntryEnable
        {
            get
            {
                return _IPEntryEnable;
            }
            set
            {
                if (_IPEntryEnable != value)
                {
                    _IPEntryEnable = value;
                    OnPropertyChanged("IPEntryEnable");
                }
            }
        }
        private bool _PortEntryEnable;
        public bool PortEntryEnable
        {
            get
            {
                return _PortEntryEnable;
            }
            set
            {
                if (_PortEntryEnable != value)
                {
                    _PortEntryEnable = value;
                    OnPropertyChanged("PortEntryEnable");
                }
            }
        }
        private bool _IsOnDevice;
        public bool IsOnDevice
        {
            get
            {
                return _IsOnDevice;
            }
            set
            {
                if (_IsOnDevice != value)
                {
                    _IsOnDevice = value;
                    OnPropertyChanged("IsOnDevice");
                    if (IsOnDevice)
                    {
                        IPEntryEnable = false;
                        PortEntryEnable = false;
                    }
                    else
                    {
                        IPEntryEnable = true;
                        PortEntryEnable = true;
                    }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private AppSettings ipAddressSetting;
        private AppSettings portSetting;
        private AppSettings autoConnectSetting;
        private AppSettings isOnDevice;

        public ConnectEditViewModel()
        {
            SetTheme();
            SetValues();
            GoBackCommand = new Command(
            execute: () =>
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            },
            canExecute: () =>
            {
                return true;
            });

            ConnectCommand = new Command(
            execute: () =>
            {
                SaveValues();

                Connect();

            },
            canExecute: () =>
            {
                return true;
            });

        }
        private bool isSettingsEmpty()
        {
            if (ipAddressSetting != null && portSetting != null && autoConnectSetting != null && isOnDevice != null)
            {
                return false;
            }
            return true;
        }
        private async void SetValues()
        {
            ipAddressSetting = await App.SQLiteDb.ReadAppSetting("IPAddress");
            portSetting = await App.SQLiteDb.ReadAppSetting("Port");
            autoConnectSetting = await App.SQLiteDb.ReadAppSetting("AutoConnect");
            isOnDevice = await App.SQLiteDb.ReadAppSetting("IsOnDevice");
            var invoiceNum = await App.SQLiteDb.ReadAppSetting("InvoiceNum");
            var sysNum = await App.SQLiteDb.ReadAppSetting("SysNum");
            if (invoiceNum == null)
            {
                AppSettings invoiceNumSetting = new AppSettings()
                {
                    Name = "InvoiceNum",
                    Value = "1"
                };
                App.SQLiteDb.InsertAppSetting(invoiceNumSetting);
            }
            if (sysNum == null)
            {
                AppSettings invoiceNumSetting = new AppSettings()
                {
                    Name = "SysNum",
                    Value = "1"
                };
                App.SQLiteDb.InsertAppSetting(invoiceNumSetting);
            }
            if (isOnDevice == null)
            {
                AppSettings isOnDeviceSetting = new AppSettings()
                {
                    Name = "IsOnDevice",
                    Value = "false"
                };
                App.SQLiteDb.InsertAppSetting(isOnDeviceSetting);
            }
            if (isSettingsEmpty())
            {
                IPAddress = "192.168.1.1";
                Port = "6001";
                IsAutoConnect = false;
                IsOnDevice = false;
            }
            else
            {
                IPAddress = ipAddressSetting.Value;
                Port = portSetting.Value;
                IsAutoConnect = Convert.ToBoolean(autoConnectSetting.Value);
                IsOnDevice = Convert.ToBoolean(isOnDevice.Value);
            }
            if (IsOnDevice)
            {
                IPEntryEnable = false;
                PortEntryEnable = false;
            }
            else
            {
                IPEntryEnable = true;
                PortEntryEnable = true;
            }
        }
        private async void SaveValues()
        {
            AppSettings ip = new AppSettings();
            ip.Name = "IPAddress";
            ip.Value = IPAddress;

            AppSettings port = new AppSettings();
            port.Name = "Port";
            port.Value = Port;

            AppSettings autoConnect = new AppSettings();
            autoConnect.Name = "AutoConnect";
            autoConnect.Value = IsAutoConnect.ToString();

            AppSettings isOnDev = new AppSettings();
            isOnDev.Name = "IsOnDevice";
            isOnDev.Value = IsOnDevice.ToString();

            if (isSettingsEmpty())
            {
                await App.SQLiteDb.InsertAppSetting(ip);
                await App.SQLiteDb.InsertAppSetting(port);
                await App.SQLiteDb.InsertAppSetting(autoConnect);
                await App.SQLiteDb.InsertAppSetting(isOnDev);
            }
            else if (ip.Value != ipAddressSetting.Value)
            {
                ip.Id = ipAddressSetting.Id;
                await App.SQLiteDb.UpdateAppSetting(ip);
            }
            else if (port.Value != portSetting.Value)
            {
                port.Id = portSetting.Id;
                await App.SQLiteDb.UpdateAppSetting(port);
            }
            else if (autoConnect.Value != autoConnectSetting.Value)
            {
                autoConnect.Id = autoConnectSetting.Id;
                await App.SQLiteDb.UpdateAppSetting(autoConnect);
            }
            if (isOnDev.Value != isOnDevice.Value)
            {
                isOnDev.Id = isOnDevice.Id;
                await App.SQLiteDb.UpdateAppSetting(isOnDev);
            }

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
        private async void Connect()
        {
            UserDialogs.Instance.Toast("Łączenie...");
            IPAddress ipAddress = System.Net.IPAddress.Parse("192.168.1.1");
            try
            {
                if (IsOnDevice)
                 {
                    ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                 }
                 else
                 {
                ipAddress = System.Net.IPAddress.Parse(IPAddress);
                 }

            }
            catch
            {
                UserDialogs.Instance.Toast("Wpisano niepoprawny adres IP");
                return;
            }

            await Task.Run(() => TCPConnection.StartTCPClient(ipAddress, IsOnDevice ? 6001 : int.Parse(Port)));
            await Task.Delay(500);

            if (TCPConnection.clientSocket.Connected)
            {

                await Application.Current.MainPage.Navigation.PopModalAsync();
                UserDialogs.Instance.Toast("Połączono poprawnie.");
            }
            else
            {
                if (await UserDialogs.Instance.ConfirmAsync("Czy kontunuować w trybie OFFLINE?\n(brak możliwośći wydruków)", "Nie udało się nawiązać połączenia!", "Tak", "Nie"))
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    UserDialogs.Instance.Toast("Sprawdź poprawność połączenia i spróbuj ponownie.");
                }
            }
        }
    }
}
