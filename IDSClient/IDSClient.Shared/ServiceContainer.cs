using GalaSoft.MvvmLight.Ioc;
using IDSClient.Shared.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDSClient.Shared
{
    public static class ServiceContainModule
    {
        public static void Init(ISimpleIoc serviceProvider)
        {
            serviceProvider.Register<IStackNavService>(() =>
           {
               var navigationService = new StackNavigationService();

               navigationService.Configure(nameof(MainPage), typeof(MainPage));
               navigationService.Configure(nameof(SupplierListView), typeof(SupplierListView));
               navigationService.Configure(nameof(SupplierItemView), typeof(SupplierItemView));


               return navigationService;
           });
        }
    }
}
