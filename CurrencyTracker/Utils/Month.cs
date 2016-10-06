using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyTracker.Utils
{
    public struct Month
    {
        public string Name { get; }
        public int Number { get; }

        public Month(string name, int number)
        {
            Name = name;
            Number = number;
        }
    }
}