using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CurrencyTracker.WebService;
using System.Data;
using CurrencyTracker.Utils;
using System.Globalization;

namespace CurrencyTracker.Controllers
{
    public class HomeController : Controller
    {
        const string ENUM_VALUTES_KEY = "EnumValutes";
        const string CURRENCY_DYNAMIC_KEY = "ValuteCursDynamic";
        const string RARE_CURRENCY_DYNAMIC_KEY = "VCD";
        const string VALUE_KEY = "Vcurs";
        const string DATE_KEY = "CursDate";
        const string RARE_VALUE_KEY = "val";
        const string RARE_DATE_KEY = "DT";

        private DailyInfoSoapClient ServiceClient { get; }

        public HomeController() : base()
        {
            this.ServiceClient = new DailyInfoSoapClient();
        }

        public ActionResult Index()
        {
            DataSet currenciesData = ServiceClient.EnumValutes(true);

            DataTable table = currenciesData.Tables[ENUM_VALUTES_KEY];

            List<Currency> currencies = new List<Currency>();

            foreach (DataRow currencyData in table.Rows)
            {
                currencies.Add(new Currency(currencyData));
            }

            ViewBag.Currencies = currencies;

            ViewBag.Months = getMonths();

            return View();
        }

        private Month[] getMonths()
        {
            string[] monthsNames = DateTimeFormatInfo.CurrentInfo.MonthNames;

            Month[] months = new Month[12];
            int monthNumber = 0;
            foreach (string monthName in monthsNames)
            {
                if (!String.IsNullOrEmpty(monthName))
                {
                    months[monthNumber++] = new Month(monthName, monthNumber);
                }
            }
            return months;
        }

        public ActionResult Chart(string currency, int month)
        {
            string[] currencyData = currency.Split(';');

            string currencyCode = currencyData[0];
            string currencyNumberCode = currencyData[1];

            DateTime startDate = new DateTime(DateTime.Now.Year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            List<string> days = new List<string>();

            DataSet dynamic = ServiceClient.GetCursDynamic(startDate, endDate, currencyCode);
            DataTable table = dynamic.Tables[CURRENCY_DYNAMIC_KEY];
            string valueKey = VALUE_KEY;
            string dateKey = DATE_KEY;
            if (table.Rows.Count == 0 && !string.IsNullOrWhiteSpace(currencyNumberCode))
            {
                dynamic = ServiceClient.GetReutersCursDynamic(startDate, endDate, int.Parse(currencyNumberCode));
                table = dynamic.Tables[RARE_CURRENCY_DYNAMIC_KEY];
                valueKey = RARE_VALUE_KEY;
                dateKey = RARE_DATE_KEY;
            }

            List<double> data = new List<double>();

            if (table.Rows.Count == 0)
            {
                return View("ChartError");
            }

            foreach (DataRow item in table.Rows)
            {
                data.Add(double.Parse(item[valueKey].ToString()));
                days.Add(((DateTime) item[dateKey]).ToString("MM-dd-yyyy"));
            }

            ViewBag.Data = data;
            ViewBag.Days = days;

            return View();
        }
        
        
    }
}