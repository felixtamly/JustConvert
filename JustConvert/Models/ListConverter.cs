using System;
using System.Collections.Generic;

namespace JustConvert.Models
{
    public class ListConverter
    {
        readonly List<Currency> CurrencyList;
        public ListConverter(List<Currency> CurrencyList)
        {
            this.CurrencyList = CurrencyList;
        }
        public List<Currency> Convert(string TargetName)
        {
            Converter Converter = new Converter(CurrencyList);
            List<Currency> ConvertedList = new List<Currency>();
            foreach (Currency Currency in CurrencyList)
            {
                double NewRate = 1 / (Converter.Convert(TargetName, Currency.Name, 1));
                ConvertedList.Add(new Currency(Currency.Name, Math.Round(NewRate, 4)));
            }
            return ConvertedList;
        }
    }
}
