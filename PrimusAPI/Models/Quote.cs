using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimusAPI.Models
{
    public class QuoteRequest
    {
        public string originCity { get; set; }
        public string originState { get; set; }
        public string originZipcode { get; set; }
        public string originCountry { get; set; }
        public string destinationCity { get; set; }
        public string destinationState { get; set; }
        public string destinationZipcode { get; set; }
        public string destinationCountry { get; set; }
        public string uOM { get; set; }
        public string pickupDate { get; set; }
        public string equipment { get; set; }
        public List<string> Accessorials { get; set; }
        public JArray freightInfo { get; set; }
    }

    public class FreightInfo
    {
        public int qty { get; set; }
        public int weight { get; set; }
        public string weightType { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string Class { get; set; }
        public int hazmat { get; set; }
        public string commodity { get; set; }
        public string dimType { get; set; }
        public string stack { get; set; }
    }

    public class QuoteResponse
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public Result results { get; set; }
        public string message { get; set; }
    }
    public class Result
    {
        public List<Rate> rates { get; set; }
        public List<object> noRates { get; set; }
    }
    public class Rate
    {
        public string id { get; set; }
        public string name { get; set; }
        public string SCAC { get; set; }
        public string serviceLevel { get; set; }
        public string serviceLevelCode { get; set; }
        public int transitDays { get; set; }
        public List<RateBreakdown> rateBreakdown { get; set; }
        public List<object> rateRemarks { get; set; }
        public decimal total { get; set; }
        public string rateType { get; set; }
        public string iconUrl { get; set; }
        public List<object> freightInfoReclassed { get; set; }
        public decimal responseTime { get; set; }
        public int vendorId { get; set; }
    }

    public class RateBreakdown
    {
        public string name { get; set; }
        public decimal total { get; set; }
    }

    public class SaveQuoteRequest
    {
        public string rateId { get; set; }
        public int originShippingLocationId { get; set; }
        public int destinationShippingLocationId { get; set; }
        public decimal laneDistance { get; set; }
    }

    public class SaveQuoteResponse
    {
        public SaveQuote data { get; set; }
    }

    public class SaveQuote
    {
        public SaveQuoteResults results { get; set; }
        public string message { get; set; }
    }

    public class SaveQuoteResults
    {
        public int quoteId { get; set; }
        public string quoteNumber { get; set; }
        public string url { get; set; }
    }
}
