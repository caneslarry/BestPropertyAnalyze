using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using BestPropertyAnalyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BestPropertyAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        // This would come from a ZillowAPI 
        public const double RestEstimate = 1500.00;
        public const double Mortgage = 1100.00;
        public const double Tax = 00.00;
        public const double Insurance = 100.00;

        private string[] ignoreList = { "PropertyRecords", "details", "Building_Area","parcel_snapshot" };

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
                    // Grab data from qurantarium API for the address/citystatezip
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

            // Get more details by the property id
            url = "https://qvmservices-test.quantarium.com/QDataService/GetPropertyRecordsXml?u=KnoxHoldings-Test&k=fP0uwn;K73Jgs=dg&id=" + house.id;

            XDocument xmlDoc = XDocument.Load(url);

            foreach (var node in xmlDoc.Descendants())
            {
                if (!ignoreList.Contains<string>(node.Name.LocalName))
                {
                    // TODO: Take the markup out of the controller
                    house.Details += "<div class='card col-sm-6'><label>" + node.Name.LocalName + "</label>" + node.Value + "</div>";
                }
            }

            return house;

        }

    }
}
