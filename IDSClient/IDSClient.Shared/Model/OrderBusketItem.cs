using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDSClient.Shared.Model
{
    public class OrderBusketItem : INotifyPropertyChanged
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

        private OrderBusket orderBusket;
        public OrderBusket OrderBusket
        {
            get
            {
                return this.orderBusket;
            }
            set
            {
                if (value != orderBusket)
                {
                    this.orderBusket = value;
                    OnPropertyChanged();
                }
            }
        }



        private Good good;
        public Good Good
        {
            get
            {
                return this.good;
            }
            set
            {
                if (value != good)
                {
                    this.good = value;
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
