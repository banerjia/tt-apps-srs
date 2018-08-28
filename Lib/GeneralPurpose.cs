using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace tt_apps_srs.Lib
{
    public static class GeneralPurpose
    {
        public static GoogleGeocoding_Location GetLatLong(string address)
        {
            GoogleGeocoding_Location retval = new GoogleGeocoding_Location(); 
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyA47sqq4fHReCXHT3UTFGaLgulC5sooxt8&address={0}&sensor=false",
     Uri.EscapeDataString(address));

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var response = (HttpWebResponse)request.GetResponse();
            string content = String.Empty;
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }

            }
            var results = (JArray)((JObject.Parse(content))["results"]);
            var attribs = results.First;

            retval.lat = (double)attribs["geometry"]["location"]["lat"];
            retval.lng = (double)attribs["geometry"]["location"]["lng"];

            return retval;
        }
    }

    public struct GoogleGeocoding_Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
