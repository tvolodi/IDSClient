using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using IDCommon;
using IDSClient.Shared.Connectors;
using IDSClient.Shared.Lib;
using IDSClient.Shared.Model;
using IDSClient.Shared.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDSClient.Shared.ViewModel
{
    [Windows.UI.Xaml.Data.Bindable]
    public class SupplierListViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IStackNavService navService;

        private string currentView;
        public string CurrentView
        {
            get => currentView;
            set => Set(() => CurrentView, ref currentView, value);
        }

        public ICommand ToSupplierItemPageCmd { get; }

        public ICommand ReloadPageCmd { get; }

        public ICommand AddSupplierItemCmd { get; }

        public ICommand DeleteSupplierItemCmd { get; }

        public ICommand UpdateSupplierItemCmd { get; }

        public ICommand ViewSupplierItemCmd { get; }

        public ObservableCollection<Supplier> SupplierObsCollection = new ObservableCollection<Supplier>();

        public SupplierListViewModel()
        {
            navService = SimpleIoc.Default.GetInstance<IStackNavService>();

            ToSupplierItemPageCmd = new RelayCommand<Supplier>(item => navService.NavigateTo(nameof(SupplierItemView), item));

            AddSupplierItemCmd = new RelayCommand<Supplier>(item => navService.NavigateTo(nameof(SupplierItemView), new OperationArg
            {
                DataContext = this,
                DataObject = item,
                Operation = Operation.Insert
            }));

            DeleteSupplierItemCmd = new RelayCommand<Supplier>(item => navService.NavigateTo(nameof(SupplierItemView), new OperationArg
            {
                DataContext = this,
                DataObject = item,
                Operation = Operation.Delete
            }));

            UpdateSupplierItemCmd = new RelayCommand<Supplier>(item => navService.NavigateTo(nameof(SupplierItemView), new OperationArg
            {
                DataContext = this,
                DataObject = item,
                Operation = Operation.Update
            }));

            ViewSupplierItemCmd = new RelayCommand<Supplier>(item => navService.NavigateTo(nameof(SupplierItemView), new OperationArg
            {
                DataContext = this,
                DataObject = item,
                Operation = Operation.Read
            }));

            // ReloadPageCmd = new AsyncCommand(async () => await  )

            CurrentView = "SupplierListView";

            for (int cnt = 0; cnt < 10; cnt++)
            {
                SupplierObsCollection.Add(
                    new Supplier
                    {
                        Id = cnt,
                        Code = cnt.ToString(),
                        Name = $"Customer {cnt}",
                        PhoneNo = $"Phone no {cnt}"
                    });

                ConnectionStatus = "Disconnected";
            }

            // SelectedItem = SupplierObsCollection[0];
        }

        private Supplier _selectedItem = null;
        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }

        private string _connectionStatus = "";
        public string ConnectionStatus
        { 
            get { return _connectionStatus; } 
            set { Set(ref _connectionStatus, value); }
        }

        public async Task UpdateSuppliers()
        {
            //             List<SupplierDTO> suppliersDTOs = await HttpRequestService.GetRequestResult<List<SupplierDTO>>();
            List<SupplierDTO> suppliersDTOs = new List<SupplierDTO>();
            for(int i = 0; i<10; i++)
            {
                suppliersDTOs.Add(
                    new SupplierDTO
                    {
                        Id = 1,
                        Name = "Supplier" + i,
                        CreatedDT = DateTimeOffset.Now                        
                    });
            }

            SupplierObsCollection.Clear();
            for(int cnt = 0; cnt < suppliersDTOs.Count; cnt++)
            {
                SupplierObsCollection.Add(Mappers.SupplierMapper.DTOToSupplier(suppliersDTOs[cnt]));
            }

            //HttpClient httpClient = new HttpClient();

            //var httpResponse = await httpClient.GetAsync("http://localhost:5000/api/suppliers");

            //var statusCode = httpResponse.StatusCode;

            //var content = httpResponse.Content.ReadAsStringAsync();
        }

        //protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (Equals(field, value)) return false;

        //    field = value;

        //    RaisePropertyChanged(propertyName);

        //    return true;
        //}

        public async Task<int> UpdateEntityAsync(Supplier supplier)
        {
            int result = 0;



            return result;
        }

        public async Task<int> InsertEntityAsync(Supplier supplier)
        {
            int result = 0;

            SupplierObsCollection.Add(supplier);
            SelectedItem = supplier;

            return result;
        }
    }
}
