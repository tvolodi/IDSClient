using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace IDSClient.Shared.Lib
{
    public class OperationArg
    {
        // Parent View Frame
        public Frame Frame { get; set; }

        // Operation type
        public Operation Operation { get; set; }

        // View model which manages data
        public object DataContext { get; set; } 

        // Data object for the operation
        public object DataObject { get; set; }
    }
}
