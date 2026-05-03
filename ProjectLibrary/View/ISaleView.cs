using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.Model;
using ProjectLibrary.Models;

namespace ProjectLibrary.View
{
    public interface ISaleView
    {
        void DisplaySales(BindingList<Sale> sales);
        void DisplayPhonesForSale(BindingList<Phone> phones);
        void ShowMessage(string message);
        void ShowReceipt(Sale sale, Phone phone);
        void RefreshData();
        string GetSelectedPhoneId();
        int GetQuantity();
    }
}
