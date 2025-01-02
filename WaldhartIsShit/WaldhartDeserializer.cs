namespace WaldhartIsShit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
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

                            string regexString = @"journal_id=(\d+)";
                            string regexString1 = @"course_id=(\d+)";
                            Regex regex = new Regex(regexString);
                            Regex regex1 = new Regex(regexString1);

                            string journalIdResult = regex.Match(MoreInfoUrl).Groups[1].Value;
                            string courseIdResult = regex1.Match(MoreInfoUrl).Groups[1].Value;
                            Console.WriteLine("===================================");
                            Console.WriteLine($"JournalID: {journalIdResult}");
                            Console.WriteLine($"CourseID: {courseIdResult}");
                            Console.WriteLine("===================================");
                            Console.WriteLine("\n");

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
