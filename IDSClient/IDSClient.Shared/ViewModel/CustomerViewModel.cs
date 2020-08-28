using IDSClient.Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDSClient.Shared.ViewModel
{
    public class CustomerViewModel
    {
        public ObservableCollection<Customer> Customers = new ObservableCollection<Customer>();

        public void UpdateCustomers()
        {
            for(int cnt = 0; cnt<10; cnt++)
            {
                Customers.Add(
                    new Customer
                    {
                        Id = Guid.NewGuid(),
                        Name = "Customer ${cnt}",
                        PhoneNo = "Phone no ${cnt}"
                    });
            }
        }
    }
}
