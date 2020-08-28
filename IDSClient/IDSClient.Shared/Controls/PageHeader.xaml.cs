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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IDSClient.Shared.Controls
{
    public enum HeaderMode
    {
        Minimal = 10,
        Large = 20
    }

    public sealed partial class PageHeader : BaseUserControl
    {
        public PageHeader()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler HamburgerClick;

        public HeaderMode Mode
        {
            get => (HeaderMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
            nameof(Mode),
            typeof(HeaderMode),
            typeof(PageHeader),
            new PropertyMetadata(HeaderMode.Large));

        public static readonly DependencyProperty HamburgerMenuVisibilityProperty =
            DependencyProperty.Register(
                nameof(HamburgerMenuVisibility),
                typeof(Visibility),
                typeof(PageHeader),
                new PropertyMetadata(Visibility.Visible)
                );

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(PageHeader),
            new PropertyMetadata(string.Empty));


        public Visibility HamburgerMenuVisibility
        {
            get => (Visibility)GetValue(HamburgerMenuVisibilityProperty);
            set => SetValue(HamburgerMenuVisibilityProperty, value);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            HamburgerClick?.Invoke(sender, e);
        }
    }
}
