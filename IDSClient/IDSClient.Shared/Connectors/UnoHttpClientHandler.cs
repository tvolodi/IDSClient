using System.Net.Http;

#if __WASM__
using Uno.UI.Wasm;
#elif __ANDROID__
using Xamarin.Android.Net;
#endif

namespace IDSClient.Shared.Connectors
{
    public class UnoHttpClientHandler :
#if __WASM__
		WasmHttpHandler
#elif __IOS__
		NSUrlSessionHandler
#elif __ANDROID__
		AndroidClientHandler
#else
        HttpClientHandler
#endif
    {
        // base
    }
}
