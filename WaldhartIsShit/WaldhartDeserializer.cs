namespace WaldhartIsShit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HtmlAgilityPack;

    public static class WaldhartDeserializer
    {

        public static void ConvertWaldhartResponse(string response)
        {
            string WaldhartUrl = "https://kvitfjell-desk.skischoolshop.com";
            var HtmlDoc = new HtmlDocument();

            HtmlDoc.LoadHtml(response);

            foreach (var DayNode in HtmlDoc.DocumentNode.SelectNodes("//li[@data-role='list-divider']"))
            {
                string DayNodestr = DayNode.InnerText.Trim();
                Console.WriteLine($"Day Node:   {DayNodestr}");


                // Find courses under this date
                var nextSibling = DayNode.NextSibling;


                while (nextSibling != null && !nextSibling.GetAttributeValue("data-role", "").Equals("list-divider"))
                {
                    if (nextSibling.Name == "li")
                    {
                        if (nextSibling.SelectSingleNode(".//a") != null)
                        {
                            string MoreInfoUrl = nextSibling.SelectSingleNode(".//a").GetAttributeValue("href", "");
                            string CourseString = nextSibling.SelectSingleNode(".//a").InnerText.Trim();
                            //Console.WriteLine(WaldhartUrl + MoreInfoUrl);
                            //Console.WriteLine($"Course:    {CourseString}");

                        }
                    }

                    nextSibling = nextSibling.NextSibling;
                }

            }

        }

    }
}
