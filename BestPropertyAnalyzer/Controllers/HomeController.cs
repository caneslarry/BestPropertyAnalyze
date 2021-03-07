using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BestPropertyAnalyzer.Models;
using System.Collections.Specialized;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Xml;
using Zillow.Services;
using Zillow.Services.Schema;
using System.Text.Json;
using System.Collections;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BestPropertyAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public const double RestEstimate = 1500.00;
        public const double Mortgage = 1100.00;
        public const double Tax = 00.00;
        public const double Insurance = 100.00;

        private string[] ignoreList = { "PropertyRecords", "details", "Building_Area" };

        private const string ZWSID = "X1-ZWz16rr2tjewwb_9hc6e";

        public const string ZillowRoot = "http://www.zillow.com/webservice/GetSearchResults.htm";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ListingData GetPropertyData(string address, string citystatezip)
        {
            var jsonData = string.Empty;

            var url = "https://qvmservices-test.quantarium.com/QDataService/QueryPropertiesByAddress?u=KnoxHoldings-Test&k=fP0uwn;K73Jgs=dg&address=" + address + "&citystate_zip=" + citystatezip;

            ListingData[] data = null;
            ListingData house = null;


            using (var webClient = new WebClient())
            {
                try
                {
                    jsonData = webClient.DownloadString(url);

                    //TODO: jsonData can be null

                    data = System.Text.Json.JsonSerializer.Deserialize<ListingData[]>(jsonData);

                    house = data[0];


                }
                catch (Exception e)
                {
                    ;
                }


            }

            url = "https://qvmservices-test.quantarium.com/QDataService/GetPropertyRecordsXml?u=KnoxHoldings-Test&k=fP0uwn;K73Jgs=dg&id=" + house.id;

            XDocument xmlDoc = XDocument.Load(url);

            foreach (var node in xmlDoc.Descendants())
            {
                if (!ignoreList.Contains<string>(node.Name.LocalName))
                {
                    house.Details += node.Name.LocalName + ": " + node.Value + "<br/>";
                }
            }

            return house;

        }

    }
}
