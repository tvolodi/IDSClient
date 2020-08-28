using IDSClient.Shared.Model;
using IDSClient.Shared.View;
using IDSClient.Shared.Lib;
using IDSClient.Shared.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class SupplierListView : Page
    {
        public SupplierListViewModel EntityListViewModel;

        public string ConnectionStatus { get; set; }


        public SupplierListView()
        {
            this.InitializeComponent();

            //if(GlobalDataTools.GlobalContextDict.ContainsKey("SuppliersViewModel"))
            //{
            //    EntityListViewModel = GlobalDataTools.GlobalContextDict["SuppliersViewModel"] as SuppliersViewModel;
            //} else
            //{
                
            //    // GlobalDataTools.GlobalContextDict["SuppliersViewModel"] = EntityListViewModel;

            //}

            EntityListViewModel = new SupplierListViewModel();

            if (EntityListViewModel.SupplierObsCollection.Count > 0)
            {
                EntityListViewModel.SelectedItem = EntityListViewModel.SupplierObsCollection[0];
            }


            this.DataContext = EntityListViewModel;

            EntityListView.ItemsSource = EntityListViewModel.SupplierObsCollection;

        }

        private void ItemTappedHndl(object sender, TappedRoutedEventArgs e)
        {
            SetControlVisibility();
        }

        private void ItemDoubleTappedHndl(object sender, DoubleTappedRoutedEventArgs e)
        {

        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(SupplierItemView));
        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SupplierItemView),
                                new OperationArg { Operation = Operation.Update,
                                    DataContext = EntityListViewModel,
                                    DataObject = EntityListViewModel.SelectedItem,
                                    Frame = this.Frame
                                });
        }

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = EntityListView.SelectedItems;
            int selectedItemCount = selectedItems.Count;

            ContentDialog deleteContentDialog = new ContentDialog
            {
                Title = "Delete the supplier?",
                Content = $"You are going to delete {selectedItemCount} items. Continue?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult contentDialogResult = await deleteContentDialog.ShowAsync();

            if (contentDialogResult == ContentDialogResult.Primary)
            {
                for (int cnt = 0; cnt < selectedItemCount; cnt++)
                {
                    Supplier supplier = (Supplier)selectedItems[0];
                    EntityListViewModel.SupplierObsCollection.Remove(supplier);
                    int newItemCnt = selectedItems.Count;
                }
            }
        }

        public void FindButton_Click(object sender, RoutedEventArgs e)
        {
            EntityListViewModel.SelectedItem = EntityListViewModel.SupplierObsCollection[3];
        }


        private void SetControlVisibility()
        {
            if (EntityListView.SelectedItems.Count == 0)
            {
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            if (EntityListView.SelectedItems.Count == 1)
            {
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else if (EntityListView.SelectedItems.Count > 1)
            {
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//             SetControlVisibility();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                EntityListViewModel = new SupplierListViewModel();
                await EntityListViewModel.UpdateSuppliers();
            }
            else
            {
                OperationArg operationArg = e.Parameter as OperationArg;
                EntityListViewModel = operationArg.DataContext as SupplierListViewModel;
            }

            this.DataContext = EntityListViewModel;

            EntityListView.ItemsSource = EntityListViewModel.SupplierObsCollection;

            if (EntityListViewModel.SupplierObsCollection.Count > 0)
            {
                EntityListViewModel.SelectedItem = EntityListViewModel.SupplierObsCollection[0];
            }

            SetControlVisibility();
        }
    }


}
