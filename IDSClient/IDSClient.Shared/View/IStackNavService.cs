using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace IDSClient.Shared.View
{
    public delegate void OnNavigatingEventHandler(IStackNavService sender, OnNavigatingEventArgs args);

    public delegate void BackButtonPressEventHandler(IStackNavService sender, BackRequestedEventArgs args);

    public delegate void BackButtonDoublePressEventHandler(IStackNavService sender, BackRequestedEventArgs args);

    /// <summary>
    /// Interface to 
    /// </summary>
    public interface IStackNavService : INavigationService
    {
        event BackButtonPressEventHandler BackButtonPressed;

        event BackButtonDoublePressEventHandler BackButtonDoublePressed;

        event OnNavigatingEventHandler OnNavigating;

        IList<PageStackEntry> BackStack { get; }

        void NavigateToAndClearStack(string pageKey, object parameter = null);

        void NavigateToAndRemoveSelf(string pageKey, object parameter = null);

        void GoBackTo(string pageKey);
    }
}
