using System;
using System.Collections.Generic;

namespace JustConvert.Models
{
    public class Converter
    {
        readonly List<Currency> CurrencyList;
        public Converter(List<Currency> CurrencyList)
        {
            this.CurrencyList = CurrencyList;
        }
        public double Convert(string TargetCurrency, string OriginalCurrency, double OriginalValue)
        {
            RateFinder RateFinder = new RateFinder(CurrencyList);
            double OriginalRate = RateFinder.FindRate(OriginalCurrency);
            double TargetRate = RateFinder.FindRate(TargetCurrency);
            return OriginalValue * TargetRate / OriginalRate;
        }
    }
}
