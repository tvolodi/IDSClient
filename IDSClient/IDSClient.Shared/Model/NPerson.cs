using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDSClient.Shared.Model

{
    public class NPerson : INotifyPropertyChanged
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

        private void OnPropertyChanged([CallerMemberName] string member = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
