using Acr.UserDialogs;
using SalesApp.Effects;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SalesApp.ViewModels
{
    public class MeasuresViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Command SaveCommand { get; set; }
        public Command GoBackCommand { get; set; }

        private string _MeasureShortNameTxt;

        private string _MeasureFullNameTxt;

        private ObservableCollection <Units> _UnitsList;

        public ObservableCollection<Units> UnitsList
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

        public string MeasureShortNameTxt
        {
            get
            {
                return _MeasureShortNameTxt;
            }
            set
            {
                if (_MeasureShortNameTxt != value)
                {
                    _MeasureShortNameTxt = value;
                    OnPropertyChanged("MeasureShortNameTxt");
                }
            }
        }

        public string MeasureFullNameTxt
        {
            get
            {
                return _MeasureFullNameTxt;
            }
            set
            {
                if (_MeasureFullNameTxt != value)
                {
                    _MeasureFullNameTxt = value;
                    OnPropertyChanged("MeasureFullNameTxt");
                }
            }
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new Command(async (item) =>
                {
                    var clicked = item as Units;
                    if (await UserDialogs.Instance.ConfirmAsync($"Czy usunąć {clicked.Name}? \n\nPamiętaj by przed usunięciem zmienić jednostkę w towarach, które mają ją przypisaną!", "Usuń", "Tak", "Nie"))
                    {

                        var goodWithUnitExist = await App.SQLiteDb.ReadGoodyUnitId(clicked.Id); 
                        if(goodWithUnitExist == null)
                        {
                            await App.SQLiteDb.DeleteUnit(clicked);
                            UnitsList.Remove(clicked);
                            UserDialogs.Instance.Toast("Usunięto");
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Nie można usunąć. Jednostka nadal jest przypisana do towarów.");
                        }

                    }                  
                });
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
        public MeasuresViewModel()
        {
            SetTheme();
            ReadAllUnits();
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(
            execute: () =>
            {
                SaveValues();
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
        private async void SaveValues()
        {
            if(MeasureShortNameTxt != "" && MeasureFullNameTxt != "" && MeasureShortNameTxt != null && MeasureFullNameTxt != null)
            {
                Units unit = new Units();
                unit.Name = MeasureFullNameTxt;
                unit.ShortCut = MeasureShortNameTxt;
                await App.SQLiteDb.InsertUnit(unit);
                UserDialogs.Instance.Toast("Zapisano pomyślnie");
                MeasureFullNameTxt = "";
                MeasureShortNameTxt = "";
                UnitsList.Add(unit);
            }
            else
            {
                UserDialogs.Instance.Toast("Najpierw uzupełnij oba pola");
            }
            
        }
        private async void ReadAllUnits()
        {
            UnitsList = new ObservableCollection<Units>(await App.SQLiteDb.ReadAllUnits());
        }
        private Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }

    }
}
