using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace JustConvert.Models
{
    public class XmlToCurrencyListConverter
    {
        public List<Currency> GetCurrencyList()
        {
            XmlReader XmlReader = XmlReader.Create("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
           // XmlReader XmlReader = XmlReader.Create("eurofxref-daily.xml");
            List<Currency> CurrencyList = new List<Currency>();
            List<Currency> SortedCurrencyList = new List<Currency>();
            CurrencyList.Add(new Currency("EUR", 1.0000));
            try
            {
                while (XmlReader.Read())
                {
                    if ((XmlReader.NodeType == XmlNodeType.Element) && (XmlReader.Name == "Cube") && XmlReader.GetAttribute("currency") != null)
                    {
                        string Name = XmlReader.GetAttribute("currency");
                        double Rate = Double.Parse(XmlReader.GetAttribute("rate"));
                        CurrencyList.Add(new Currency(Name, Rate));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file: {0}", e);
            }
            finally
            {
                XmlReader.Close();
            }
            SortedCurrencyList = CurrencyList.OrderBy(c => c.Name).ToList();
            return SortedCurrencyList;
        }
    }
}
