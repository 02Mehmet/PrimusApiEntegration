using Newtonsoft.Json.Linq;
using PrimusAPI.Models;
using ShipPrimus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimusAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var quoteRequest = new QuoteRequest()
            {
                originCity = "Miami",
                originState="FL",
                originZipcode = "33135",
                originCountry = "US",
                destinationCity = "Nashville",
                destinationState = "TN",
                destinationZipcode = "37203",
                destinationCountry = "US",
                uOM = "US",
                pickupDate = "06/30/2020",
                equipment = "Van",
                Accessorials = new List<string>() { "LFO", "INO", "LFD", "IND" },
                freightInfo = JArray.Parse("[{'qty':3,'weight':500,'weightType':'each','length':40,'width':48,'height':48,'class':50,'hazmat':0,'commodity':'','dimType':'PLT','stack':false}]")
            };

            var quote = await QuoteHelper.PrimusAPIGetDataPostAsync(quoteRequest);

            foreach(var item in quote.data.results.rates)
            {
                Console.WriteLine("id: " + item.id);
                Console.WriteLine("name: " + item.name);
                Console.WriteLine("SCAC: " + item.SCAC);
                Console.WriteLine("serviceLevel: " + item.serviceLevel);
                Console.WriteLine("serviceLevelCode: " + item.serviceLevelCode);
                Console.WriteLine("transitDays: " + item.transitDays);
                Console.WriteLine("total: " + item.total);
                Console.WriteLine("rateType: " + item.rateType);
                Console.WriteLine("iconUrl: " + item.iconUrl);
                Console.WriteLine("responseTime: " + item.responseTime);
                Console.WriteLine("vendorId: " + item.vendorId);
                foreach(var item1 in item.rateBreakdown)
                {
                    Console.WriteLine("Breakdown  =>>>  "+item1.name+":"+item1.total);
                }
                Console.WriteLine("-------------------------------------------------------------------------------- ");
            }
        }
    }
}
