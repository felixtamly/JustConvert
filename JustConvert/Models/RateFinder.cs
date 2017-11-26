using System;
using System.Collections.Generic;
using System.Linq;

namespace JustConvert.Models
{
    public class RateFinder
    {
        readonly List<Currency> CurrencyList;

        public RateFinder(List<Currency> CurrencyList)
        {
            this.CurrencyList = CurrencyList;
        }
        public bool CurrencyExists(string Name)
        {
            foreach (Currency Currency in CurrencyList)
            {
                if (Currency.Name.Equals(Name))
                {
                    return true;
                }
            }
            return false;
        }
        public double FindRate(string Name)
        {
            if (CurrencyExists(Name))
            {
                return (from TheCurrency in CurrencyList
                        where TheCurrency.Name.Equals(Name)
                        select TheCurrency.Rate).Single();
            }
            else
                return 0.0;
        }
    }
}
