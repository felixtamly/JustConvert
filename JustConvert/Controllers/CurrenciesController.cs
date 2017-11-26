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
            TempData["NewList"] = NewCurrencyList;
            TempData["Amount"] = 1;
            TempData["Rate"] = Name.ToUpper();
            return RedirectToAction("Index", "Currencies");
        }

        [HttpPost]
        public ActionResult GetConvertedList()
        {
            string Name = (string)TempData["Rate"];
            double Amount = 0.0;
            if (!Double.TryParse(Request["AmountToConvert"].ToString(), out Amount) || Convert.ToDouble(Request["AmountToConvert"].ToString()) <= 0)
            {
                Amount = 1.0;
            }
            else
            {
                Amount = Convert.ToDouble(Request["AmountToConvert"].ToString());
            }
            List<Currency> OriginalList = new List<Currency>();
            List<Currency> ConvertedList = new List<Currency>();
            if (TempData["NewList"] != null)
            {
                OriginalList = ListConverter.Convert((string)TempData["Rate"]);
            }
            else
            {
                OriginalList = CurrencyList;
            }
            foreach (Currency Currency in OriginalList)
            {
                Currency.Rate *= Amount;
                ConvertedList.Add(Currency);
            }
            TempData["NewList"] = ConvertedList;
            TempData["Amount"] = Amount;

            return RedirectToAction("Index", "Currencies");
        }
    }
}
