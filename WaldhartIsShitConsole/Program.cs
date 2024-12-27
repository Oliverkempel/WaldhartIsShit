﻿namespace WaldhartIsShitConsole
{
    using HtmlAgilityPack;

    using System.Runtime.CompilerServices;

    internal class Program
    {
        static void Main(string[] args)
        {
            
            
            string? htmlResponse = "<ul data-role=\"listview\" data-inset=\"true\">\r\n    <li data-role=\"list-divider\">\r\n        Morgen\r\n        Sa\r\n        28.12.2024\r\n        <span id=\"5554\" class=\"ui-li-count\" style=\"right: 1.0em !important;\"\r\n            onclick=\"change_status_to(1, null, 5554, null);change_status_to(1, null, 5496, null);change_status_to(1, 10315,null, null);change_status_to(1, null, null, 1829); return false;\">\r\n            Status:\r\n            <img src=\"/images/green_checkmark_16px.png\" alt=\"Bestätigt\" title=\"Bestätigt\"\r\n                style=\"height: 1.2em; margin-bottom: 2px\" />\r\n        </span>\r\n    </li>\r\n    <li>\r\n        <a class=\"main_link\" href=\"/mycourses/course?course_id=5554&course_day_iso=2024-12-28\">\r\n            Ø Mini duo FT\r\n            <br />\r\n            Heis G, EAST\r\n            <br />\r\n            09:30-10:30\r\n            <span class=\"ui-li-count\">\r\n                <span title=\"Teilnehmer\">RP: 0 / T: 2 / K: 2</span>\r\n            </span>\r\n        </a>\r\n    </li>\r\n    <li>\r\n        <a class=\"main_link\" href=\"/mycourses/course?course_id=5496&course_day_iso=2024-12-28\">\r\n            Jøkul duo G\r\n            <br />\r\n            Heis I, WEST\r\n            <br />\r\n            10:45-12:15\r\n            <span class=\"ui-li-count\">\r\n                <span title=\"Teilnehmer\">RP: 0 / T: 3 / K: 3</span>\r\n            </span>\r\n        </a>\r\n    </li>\r\n    <li>\r\n        <a class=\"main_link\" href=\"/mycourses/course?journal_id=10315&course_day_iso=2024-12-28\">\r\n            Private SKI 120 EAST\r\n            <br />\r\n            Jack Prent\r\n            <br />\r\n            Heis G, EAST\r\n            <br />\r\n            13:00-15:00\r\n            <span class=\"ui-li-count\">\r\n                <span title=\"Teilnehmer\">RP: 0 / T: 3</span>\r\n            </span>\r\n        </a>\r\n    </li>\r\n    <li>\r\n        <div data-role=\"popup\" id=\"popup_1829\" data-dismissible=\"false\" style=\"min-width: 320px; max-width: 90%\">\r\n            <div role=\"main\" class=\"ui-content\">\r\n                <p></p>\r\n                <br />\r\n                <div data-role=\"controlgroup\" data-type=\"horizontal\" style=\"text-align: center;\">\r\n                    <a href=\"#\" data-activity=\"1829\"\r\n                        class=\"confirm_activity ui-icon-check ui-btn-icon-left ui-btn ui-mini ui-corner-all ui-shadow ui-btn-inline ui-state-disabled\">Bestätigen</a>\r\n                    <a href=\"#\"\r\n                        class=\"ui-btn ui-corner-all ui-mini ui-shadow ui-btn-inline ui-icon-delete ui-btn-icon-left\"\r\n                        data-rel=\"back\">Schließen</a>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <a href=\"/mycourses/#popup_1829\" data-rel=\"popup\" data-transition=\"pop\" data-position-to=\"window\">\r\n            <span class=\"course_activity\" style=\"color: grey !important;\">\r\n                Jøkul\r\n                <br />\r\n                15:15-16:00\r\n            </span>\r\n        </a>\r\n    </li>\r\n    <li data-role=\"list-divider\">\r\n        So\r\n        29.12.2024\r\n        <span id=\"5554\" class=\"ui-li-count\" style=\"right: 1.0em !important;\"\r\n            onclick=\"change_status_to(1, null, 5554, null);change_status_to(1, null, 5496, null);change_status_to(0, 10288,null, null); return false;\">\r\n            Status:\r\n            <img src=\"/images/red_checkmark_16px.png\" alt=\"Nicht bestätigt\" title=\"Nicht bestätigt\"\r\n                style=\"height: 1.2em; margin-bottom: 2px\" />\r\n        </span>\r\n    </li>\r\n</ul>";

            WaldhartIsShit.WaldhartRequestHandler test = new WaldhartIsShit.WaldhartRequestHandler();

            Uri csPath = test.CreateGetCoursesUri(new DateOnly(2024, 12, 16), new DateOnly(2024, 12, 29));
            Uri cPath = test.CreateGetCourseUri(1234, new DateOnly(2024, 12, 16));
            Console.WriteLine(cPath.ToString());
            Console.WriteLine(csPath.ToString());

            //Console.WriteLine("Waldhart sucks, lets make it better");

            //ConvertWaldhartResponse(htmlResponse);

        }

        static void ConvertWaldhartResponse(string response)
        {
            string WaldhartUrl = "https://kvitfjell-desk.skischoolshop.com";
            var HtmlDoc = new HtmlDocument();

            HtmlDoc.LoadHtml(response);

            foreach(var DayNode in HtmlDoc.DocumentNode.SelectNodes("//li[@data-role='list-divider']"))
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
                            Console.WriteLine(WaldhartUrl + MoreInfoUrl);
                            //Console.WriteLine($"Course:    {CourseString}");

                        }
                    }

                    nextSibling = nextSibling.NextSibling;
                }
                


            }

        }
    }
}