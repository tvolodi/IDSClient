using System;
using System.Collections.Generic;
using System.Text;

namespace IDSClient.Shared.View
{
    public class OnNavigatingEventArgs : EventArgs
    {
        public string PageKey { get; }

        public OnNavigatingEventArgs(string pageKey)
        {
            PageKey = pageKey;
        }
    }
}
