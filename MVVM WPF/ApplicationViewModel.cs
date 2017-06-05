using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MVVM_WPF.Annotations;

namespace MVVM_WPF
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private Phone selectedPhone;

        private IFileService fileService;
        private IDialogService dialogService;

        public ObservableCollection<Phone> Phones { get; set; }

        public Phone SelectedPhone
        {
            get
            {
                return selectedPhone;
            }
            set
            {
                selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (dialogService.SaveFileDialog() == true)
                               {
                                   fileService.Save(dialogService.FilePath, Phones.ToList());
                                   dialogService.ShowMessage("File saved!");
                               }
                           }
                           catch (Exception e)
                           {
                               dialogService.ShowMessage(e.Message);
                           }
                       }));
            }
        }

        private RelayCommand openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                       (openCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (dialogService.OpenFileDialog() == true)
                               {
                                   var phones = fileService.Open(dialogService.FilePath);
                                   Phones.Clear();

                                   foreach (var p in phones)
                                   {
                                       Phones.Add(p);
                                   }

                                   dialogService.ShowMessage("File is opened");
                               }
                           }
                           catch (Exception e)
                           {
                               dialogService.ShowMessage(e.Message);
                           }
                       }));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                       (addCommand = new RelayCommand(obj =>
                       {
                           Phone phone = new Phone();
                           Phones.Insert(0, phone);
                           SelectedPhone = phone;
                       }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                       (removeCommand = new RelayCommand(obj =>
                           {
                               Phone phone = obj as Phone;
                               if (obj != null)
                               {
                                   Phones.Remove(phone);
                               }
                           },
                           (obj) => Phones.Count > 0));
            }
        }

        private RelayCommand doubleCommand;

        public RelayCommand DoubleCommand
        {
            get
            {
                return doubleCommand ??
                       (doubleCommand = new RelayCommand(obj =>
                       {
                           Phone phone = obj as Phone;
                           if (phone != null)
                           {
                               Phone phoneCopy = new Phone
                               {
                                   Company = phone.Company,
                                   Price = phone.Price,
                                   Title = phone.Title
                               };
                               Phones.Insert(0, phoneCopy);
                           }
                       }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ApplicationViewModel(IDialogService dialogService, IFileService fileService)
        {
            Phones = new ObservableCollection<Phone>
            {
                new Phone { Title="iPhone 7", Company="Apple", Price=56000 },
                new Phone {Title="Galaxy S7 Edge", Company="Samsung", Price =60000 },
                new Phone {Title="Elite x3", Company="HP", Price=56000 },
                new Phone {Title="Mi5S", Company="Xiaomi", Price=35000 }
            };

            this.dialogService = dialogService;
            this.fileService = fileService;
        }
    }
}
