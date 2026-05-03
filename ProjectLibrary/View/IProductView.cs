using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.Models;

namespace ProjectLibrary.View
{
    public interface IProductView
    {
        void DisplayPhones(BindingList<Phone> phones);
        void ShowMessage(string message);
        Phone GetSelectedPhone();
        void RefreshGrid();
    }
}
