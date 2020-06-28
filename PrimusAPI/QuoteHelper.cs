using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrimusAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShipPrimus
{
    public class QuoteHelper
    {
        public static async Task<QuoteResponse> PrimusAPIGetDataPostAsync(QuoteRequest quoteRequest)
        {
            var username = "APITest@quickload.com";
            var password = "APITest";
            var loginURL = "https://sandbox-api-applet.shipprimus.com/api/v1/login";
            var quoteURL = "https://sandbox-api-applet.shipprimus.com/applet/v1/rate/multiple";

            var token = await GetBearerTokenPostAsync(username, password, loginURL);
            var result = await GetQuotes(quoteRequest, token, quoteURL);

            return result;
        }

        public static string URLGenerator(QuoteRequest quoteRequest,string quoteURL)
        {

             quoteURL += "?originCity=" + quoteRequest.originCity + "&originState=" + quoteRequest.originState + "&originZipcode=" + quoteRequest.originZipcode + "&originCountry=" + quoteRequest.originCountry + "&destinationCity=" + quoteRequest.destinationCity + "&destinationState=" + quoteRequest.destinationState + "&destinationZipcode=" + quoteRequest.destinationZipcode + "&destinationCountry=" + quoteRequest.destinationCountry + "&freightInfo=" + quoteRequest.freightInfo.ToString() + "&UOM=" + quoteRequest.uOM + "&equipment=" + quoteRequest.equipment;

            if (quoteRequest.pickupDate != "" && quoteRequest.pickupDate != null)
            {
                //string dateTime = quoteRequest.pickupDate.ToString();
                string dateTime = Convert.ToDateTime(quoteRequest.pickupDate).ToString("yyyy-MM-dd");
                quoteURL += "&pickupDate=" + dateTime;
            }

            if(quoteRequest.Accessorials.Count()>0)
            {
                quoteURL += "&accessorialsList[]=" + PrimusAccessorialLink(quoteRequest.Accessorials);
            }

            return quoteURL;
        }

        public static string PrimusAccessorialLink(List<string> Accessorials)
        {
            var link = Accessorials[0];
            for(int i=1;i<Accessorials.Count();i++)
            {
                link += "|" + Accessorials[i];
            }
            return link;
        }

        public static async Task<string> GetBearerTokenPostAsync(string userName, string password, string loginURL)
        {
            using (var client = new HttpClient())
            {
                //Define Headers
                client.DefaultRequestHeaders.Accept.Clear();
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "Password"));
                requestData.Add(new KeyValuePair<string, string>("username", userName));
                requestData.Add(new KeyValuePair<string, string>("password", password));

                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                //Request Token
                var response = client.PostAsync(loginURL, requestBody).Result;

                var result = await response.Content.ReadAsStringAsync();
                JObject jresponse = JObject.Parse(result);

                if (response.StatusCode == System.Net.HttpStatusCode.OK && jresponse["data"] != null && jresponse["data"]["accessToken"] != null)
                {
                    var token = (string)jresponse["data"]["accessToken"];
                    return token;
                }
                else
                {
                    if (jresponse["error"] != null && jresponse["error"]["message"] != null)
                    {
                        throw new Exception(jresponse["error"]["message"].ToString());
                    }
                    else
                    {
                        throw new Exception("Error: " + result ?? "Token not taken");
                    }
                }
            }
        }

        public static async Task<QuoteResponse> GetQuotes(QuoteRequest quoteRequest, string token,string quoteURL)
        {

            using (var client = new HttpClient())
            {
                //Define Headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = URLGenerator(quoteRequest, quoteURL);

                var request = client.GetAsync(url).Result;

                var response = await request.Content.ReadAsStringAsync();
                JObject jresponse = JObject.Parse(response);
                if (request.StatusCode == System.Net.HttpStatusCode.OK && jresponse["data"] != null)
                {
                    var quoteResult = JsonConvert.DeserializeObject<QuoteResponse>(response);

                    return quoteResult;
                }
                else
                {
                    if (jresponse["error"] != null && jresponse["error"]["message"] != null)
                    {
                        throw new Exception(jresponse["error"]["message"].ToString());
                    }
                    else
                    {
                        throw new Exception("Error: " + response ?? " Rates not available");
                    }
                }
            }

        }

    }
}
