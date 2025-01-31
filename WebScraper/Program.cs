using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Collections.Generic;

// namespace to organize classes and stuff
namespace WebScraper
{
    // class program, which is our program
    class Program
    {
        // static because the method belongs to the Program class and not an object of the Program class
        static async Task Main(String[] args)
        {
            // url to scrape
            String url = "https://www.maisgasolina.com/";

            Console.WriteLine("URL: " + url);

            // httpclient to handle and get the html
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
            var html = await httpClient.GetStringAsync(url);

            Console.WriteLine("Passed initial barrier");

            // htmldocument so we can easily swift through the html code withou converting anything to strings
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // get the divs info for only brands
            var brands = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("avg-brand")).ToList();

            Console.WriteLine("Brands: " + brands);

            // get the divs info for only brands
            var prices = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("avg-price")).ToList();

            Console.WriteLine("Prices: " + prices);

            // remove the string I dont want
            for (int i = 0; i < prices.Count; i = i+2)
            {
                // i+2 because I its after 3, I remove one so after 2
                prices.RemoveAt(i);        
            }

            //  create lists for gasolina and gasoleo
            var gasolina = new List<string>();
            var gasolio = new List<string>();

            // Add gasolina and gasoleo prices respectively,
            // prices has gasolina then gasoleo price, so skip gasoleo or gasolina by adding 2 to i
            for (int i = 0; i < prices.Count; i = i+2)
            {
                gasolina.Add(prices[i].InnerText);
            }

            for (int i = 1; i < prices.Count; i = i + 2)
            {
                gasolio.Add(prices[i].InnerText.Trim());
            }

            // Make console encode in UTF8 for the euro output and not get a question mark
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // get length
            var len = brands.Count;

            // Simply organize and show each brand and prices respectively,
            // because they were already in order
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine("{0}\n" + "{1}€\n" + "{2}€\n", 
                    brands[i].InnerText.Trim(), gasolina[i].Replace('€', ':').Insert(20, " "),
                    gasolio[i].Replace('€', ':').Insert(16, " "));
            }
        }
    }
}