using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDSClient.Shared.Base
{
    public class BaseModel : INotifyPropertyChanged
    {
        private long id;
        public long Id
        {
            get { return this.id; }
            set { Set(ref id, value); }
        }

        private string code;
        public string Code
        {
            get { return this.code; }
            set { Set(ref code, value); }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set { Set(ref name, value); }
        }

        private bool isDeleted;
        public bool IsDeleted
        {
            get { return this.isDeleted; }
            set { Set(ref isDeleted, value); }
        }

        private DateTimeOffset createdDT;
        public DateTimeOffset CreatedDT
        {
            get { return this.createdDT; }
            set { Set(ref createdDT, value); }
        }

        private DateTimeOffset deletedDT;
        public DateTimeOffset DeletedDT
        {
            get { return this.deletedDT; }
            set { Set(ref deletedDT, value); }
        }

        private DateTimeOffset lastUpdatedDT;
        public DateTimeOffset LastUpdatedDT
        {
            get { return this.lastUpdatedDT; }
            set { Set(ref lastUpdatedDT, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;

            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
