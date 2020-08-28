using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDSClient.Shared.Model
{
    public class Customer : INotifyPropertyChanged
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

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != name)
                {
                    this.name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string phoneNo;
        public string PhoneNo
        {
            get
            {
                return this.phoneNo;
            }
            set
            {
                if (value != phoneNo)
                {
                    this.phoneNo = value;
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
