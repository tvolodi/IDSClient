using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDSClient.Shared.Model
{
    public class OrderBusket : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Guid id;
        public Guid Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (value != id)
                {
                    this.id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string orderNumber;
        public string OrderNumber
        {
            get
            {
                return this.orderNumber;
            }
            set
            {
                if (value != orderNumber)
                {
                    this.orderNumber = value;
                    OnPropertyChanged();
                }
            }
        }



        private Customer customer;
        public Customer Customer
        {
            get
            {
                return this.customer;
            }
            set
            {
                if (value != customer)
                {
                    this.customer = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string member = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
