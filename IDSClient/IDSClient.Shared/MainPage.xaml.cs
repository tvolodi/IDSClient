using IDSClient.Shared.View;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IDSClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MainNavigationView.IsPaneOpen = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainNavigationFrame.Navigate(typeof(DashboardView));
        }

        private void MainNavigationView_ItemInvoked(object source, NavigationViewItemInvokedEventArgs e)
        {
            string menuItemName = e.InvokedItemContainer.Name;

            if (!e.IsSettingsInvoked)
            {
                switch (menuItemName)
                {
                    case "DashboardMI":
                        break;

                    case "SuppliersMI":
                        MainNavigationFrame.Navigate(typeof(SupplierListView));
                        break;
                }
            } else
            {

            }
        }

        private void MainNavView_BackRequested(object sender, NavigationViewBackRequestedEventArgs e)
        {
            if (MainNavigationFrame.CanGoBack)
            {
                MainNavigationFrame.GoBack();
            }
        }
    }
}
