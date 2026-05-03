using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.View
{
    public interface IChartView
    {
        void DisplayRevenueChart(Dictionary<string, int> revenueByBrand);
        void ShowMessage(string message);
        DateTime GetStartDate();
        DateTime GetEndDate();
    }
}
