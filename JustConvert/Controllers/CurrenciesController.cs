using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustConvert.Models;

namespace JustConvert.Controllers
{
    public class CurrenciesController : Controller
    {
        static XmlToCurrencyListConverter XmlConverter = new XmlToCurrencyListConverter();
        static List<Currency> CurrencyList = XmlConverter.GetCurrencyList();
        static ListConverter ListConverter = new ListConverter(CurrencyList);

        public ActionResult Index()
        {
            if (TempData["NewList"] != null)
            {
                ViewData["NewAmount"] = TempData["Amount"];
                ViewData["NewRate"] = TempData["Rate"];
                TempData.Keep();
                return View((TempData["NewList"]));
            }
            //First request of list: 1 EUR = 
            else
            {
                ViewData["NewAmount"] = 1;
                ViewData["NewRate"] = "EUR";
                TempData["Rate"] = "EUR";
                return View(CurrencyList);
            }
        }

        public ActionResult GetNewList(string Name)
        {
            List<Currency> NewCurrencyList = new List<Currency>();
            NewCurrencyList = ListConverter.Convert(Name);
            TempData["NewList"] = NewCurrencyList; //To bind the new list by Index().
            TempData["Amount"] = 1;
            TempData["Rate"] = Name.ToUpper(); //Set the table header as "1 EUR"
            return RedirectToAction("Index", "Currencies");
        }

        [HttpPost]
        public ActionResult GetConvertedList()
        {
			List<Currency> OriginalList = new List<Currency>();
			List<Currency> ConvertedList = new List<Currency>();
            string Name = (string)TempData["Rate"];
            string AmountTxt = Request["AmountToConvert"];
            double Amount = 0.0;
            //USER INPUT: If user input is <= 0 or not a number, set it as 1.
            if (!Double.TryParse(AmountTxt, out Amount) || Convert.ToDouble(AmountTxt) <= 0)
            {
                Amount = 1.0;
            }
            else
            {
                Amount = Convert.ToDouble(AmountTxt);
            }
            //CONVERSION AMOUNT IS 1: If the list was not based on a conversion amount of 1, get it back to 1.
            if (TempData["NewList"] != null)
            {
                OriginalList = ListConverter.Convert((string)TempData["Rate"]);
            }
            else
            {
                OriginalList = CurrencyList;
            }
            //Multiply the table by the desired amount. 
            foreach (Currency Currency in OriginalList)
            {
                Currency.Rate *= Amount;
                ConvertedList.Add(Currency);
            }
            TempData["NewList"] = ConvertedList; //To bind the list on Index().
            TempData["Amount"] = Amount;

            return RedirectToAction("Index", "Currencies");
        }
    }
}
