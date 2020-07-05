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
            var createShipmentUrl = "https://sandbox-api-applet.shipprimus.com/applet/v1/book";
            Console.WriteLine("Get Token");
            var token = await QuoteHelper.GetBearerTokenPostAsync(username, password, loginURL);

            Console.WriteLine("Get Quotes");
            var quoteRequest = new QuoteRequest()
            {
                originCity = "Miami",
                originState="FL",
                originZipcode = "33142",
                originCountry = "US",
                destinationCity = "Laredo",
                destinationState = "TX",
                destinationZipcode = "78045",
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

            Console.WriteLine("Save Quotes");
            var quoteSaveRequest = new SaveQuoteRequest()
            {
                rateId = quote.data.results.rates[0].id,
                originShippingLocationId = 0,
                destinationShippingLocationId = 0,
                laneDistance = 0,
            };
            JObject jrequestsavequote = JObject.FromObject(quoteSaveRequest);
            var quoteSave =  QuoteHelper.SaveQuote(quoteSaveURL, token, jrequestsavequote);

            Console.WriteLine("Create Shipment");
            var createShipmentRequest = new CreateShipmentRequest()
            {
                thirdPartyReferenceNumber= "",
                PRONmbr = "",
                BOLNmbr = "testmehmet12345",
                BOLPrefix = "",
                shipper = new Location()
                {
                    id = 0,
                    name = "string",
                    referenceNumber = "1515154",
                    address1 = "string",
                    address2 = "string",
                    city = "Miami",
                    state = "FL",
                    zipCode = "33142",
                    country = "US",
                    phone = "string",
                    fax = "string",
                    email = "string",
                    contact = "string",
                    contactPhone = "string"
                },
                consignee = new Location()
                {
                    id = 0,
                    name = "string",
                    referenceNumber = "25551",
                    address1 = "string",
                    address2 = "string",
                    city = "Laredo",
                    state = "TX",
                    zipCode = "78045",
                    country = "US",
                    phone = "string",
                    fax = "string",
                    email = "string",
                    contact = "string",
                    contactPhone = "string"
                },
                quoteNumber = quoteSave.data!=null ? quoteSave.data.results.quoteNumber:"",
                lineItems = JArray.Parse("[{'qty':3,'weight':500,'weightType':'each','length':40,'width':48,'height':48,'volume': 200,'dimType':'PLT','class':'50','hazmat':0,'stack':false,'stackAmount': 1,'commodity':''}]"),
                UOM="US",
                accessorialsList = new List<string>() { "LFO", "LFO" },
                insuranceAmount = 0,
                insuranceFreight = true,
                insuranceAddOn = true,
                brokerInformation = new BrokerInformation()
                {
                    name= "",
                    contact= "",
                    phone= "",
                    notes= ""
                },
                pickupInformation = new LocationInformation()
                {
                    date = "2020-07-07",
                    type = "PO",
                    fromTime = "08:30",
                    toTime = "10:30"
                },
                deliveryInformation = new LocationInformation()
                {
                    date = "2020-07-09",
                    type = "DO",
                    fromTime = "08:30",
                    toTime = "12:30"
                },
                BOLInstructions= "",
                shipmentNotes= ""
            };
            JObject jrequestcreateshipment = JObject.FromObject(createShipmentRequest);
            var quoteCreateShipment = QuoteHelper.CreateShipment(createShipmentUrl, token, jrequestcreateshipment);
        }
    }
}
