using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CurrencyTracker.Utils
{
    public struct Currency
    {
        public string Name { get; }
        public string Code { get; }
        public string NumberCode { get; }

        const string CURRENCY_CODE = "Vcode";
        const string CURRENCY_NUMBER_CODE = "Vnumcode";
        const string CURRENCY_NAME = "Vname";

        public Currency(DataRow data)
        {
            this.Code = data[CURRENCY_CODE].ToString().Trim();
            this.Name = data[CURRENCY_NAME].ToString().Trim();
            this.NumberCode = data[CURRENCY_NUMBER_CODE].ToString().Trim();
        }
    }
}