using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Bot4App.Services
{
    public class AnaliseSentimentoEmTexto
    {
        private readonly string _textAnalisekey = ConfigurationManager.AppSettings["TextAnalysesKey"];
        private readonly string _textAnaliseUri = ConfigurationManager.AppSettings["TextAnaliseUri"];

        public async Task<string> TextAnalytics(string texto)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _textAnalisekey);

            var uri = "https://westcentralus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment?" + texto;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await client.PostAsync(uri, content);
            }
             


             
            var result = await response.Content.ReadAsStringAsync();
            var contents = XElement.Parse(result).Value;

            return $"Texto original: **{ texto }**\nTradução: **{ contents }**";
        }
    }


    public class Translate
    {
        private readonly string _translateApiKey = "0bdfbf416967418dbdfd66c3d2a12466"; //ConfigurationManager.AppSettings["TranslateApiKey"];
        private readonly string _translateUri = "https://api.microsofttranslator.com/V2/Http.svc/Translate";  //ConfigurationManager.AppSettings["TranslateUri"];

        public async Task<string> TranslateText(string texto)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translateApiKey);

            var uri = _translateUri + "?to=pt-br" +
                      "&text=" + System.Net.WebUtility.UrlEncode(texto);

            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            var content = XElement.Parse(result).Value;

            return $"Tradução: **{ content }**";
        }
    }
}