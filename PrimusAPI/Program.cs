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
            var username = "APITest@quickload.com";
            var password = "APITest";
            var loginURL = "https://sandbox-api-applet.shipprimus.com/api/v1/login";
            var quoteURL = "https://sandbox-api-applet.shipprimus.com/applet/v1/rate/multiple";
            var quoteSaveURL = "https://sandbox-api-applet.shipprimus.com/applet/v1/rate/save";
            var token = await QuoteHelper.GetBearerTokenPostAsync(username, password, loginURL);

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

            var quote = await QuoteHelper.GetQuotes(quoteRequest, token, quoteURL);

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
                Console.WriteLine("--------------------------------------------------------------------------------");
            }

            var quoteSaveRequest = new SaveQuoteRequest()
            {
                rateId = quote.data.results.rates[0].id,
                originShippingLocationId = 0,
                destinationShippingLocationId = 0,
                laneDistance = 0,
            };
            JObject jrequest = JObject.FromObject(quoteSaveRequest);

            var quoteSave =  QuoteHelper.SaveQuote(quoteSaveURL, token, jrequest);
        }
    }
}
