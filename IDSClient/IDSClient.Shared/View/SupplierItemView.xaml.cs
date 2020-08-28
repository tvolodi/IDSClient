using IDSClient.Shared.Lib;
using IDSClient.Shared.Model;
using IDSClient.Shared.View;
using IDSClient.Shared.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IDSClient.Shared.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SupplierItemView : Page
    {
        private OperationArg operationArg;

        private Supplier updatingSupplier;

        private SupplierListViewModel suppliersViewModel;

        private Operation operation;

        public SupplierItemView()
        {
            this.InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            updatingSupplier.Name = NameTB.Text;
            updatingSupplier.Code = CodeTB.Text;
            updatingSupplier.PhoneNo = PhoneNoTB.Text;

            switch (operation)
            {
                case Operation.Insert:
                    await suppliersViewModel.InsertEntityAsync(updatingSupplier);
                    break;

                case Operation.Update:
                    await suppliersViewModel.UpdateEntityAsync(updatingSupplier);
                    break;
            }

            operationArg.Frame.GoBack();
//            operationArg.DataObject = updatingSupplier;
//            operationArg.Frame.Navigate(typeof(SupplierListView), operationArg);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame parent = this.Parent as Frame;
            if (parent.CanGoBack) parent.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            operationArg = e.Parameter as OperationArg;

            suppliersViewModel = operationArg.DataContext as SupplierListViewModel;

            operation = operationArg.Operation;

            switch (operation)
            {
                case Operation.Insert:
                    updatingSupplier = new Supplier();
                    break;

                case Operation.Update:
                    updatingSupplier = operationArg.DataObject as Supplier;
                    break;
            }

            this.DataContext = updatingSupplier;

        }
    }
}
