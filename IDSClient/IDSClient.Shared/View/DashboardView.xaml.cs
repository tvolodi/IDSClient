using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
#if __WASM__
using Uno.UI.Wasm;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IDSClient.Shared.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardView : Page
    {
        public DashboardView()
        {
            this.InitializeComponent();

            DashboardSampleTextBlock.Text = "Samle text at Initialize Components";

            // GetSampleTextFromServer();

        }

        private async void GetSampleTextFromServer()
        {
            try
            {


#if __WASM__
                var innerHandler = new Uno.UI.Wasm.WasmHttpHandler();
#else
                var innerHandler = new HttpClientHandler();
#endif

                // var innerHandler2 = new System.Net.Http.WebAssemblyHttpHandler();

                // _httpClient = new HttpClient(innerHandler);
                HttpClient httpClient = new HttpClient(innerHandler);

                var httpResponse = await httpClient.GetAsync("http://192.168.1.226:5000/api/suppliers");

                var statusCode = httpResponse.StatusCode;

                var content = httpResponse.Content.ReadAsStringAsync();

                DashboardSampleTextBlock.Text = content.Result;
            }
            catch (Exception e)
            {
                DashboardSampleTextBlock.Text = e.ToString();
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
