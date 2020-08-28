using IDSClient.Shared.Base;
using System;

namespace IDSClient.Shared.Model
{
    public class Supplier : BaseModel
    {

        private string phoneNo;
        public string PhoneNo
        {
            get { return this.phoneNo; }
            set { Set(ref phoneNo, value); }
        }
    }
}
