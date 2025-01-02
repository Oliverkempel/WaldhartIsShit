namespace WaldhartIsShit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using HtmlAgilityPack;

    using WaldhartIsShit.Models.Responses;

    public static class WaldhartDeserializer
    {

        public static List<GetCoursesForecastResponse> ConvertGetCoursesForecastResponse(string response)
        {
            List<GetCoursesForecastResponse> methodResponse = new List<GetCoursesForecastResponse>();

            string WaldhartUrl = "https://kvitfjell-desk.skischoolshop.com";
            HtmlDocument? HtmlDoc = new HtmlDocument();

            HtmlDoc.LoadHtml(response);

            // Handle null exception for below code
            foreach (var DayNode in HtmlDoc.DocumentNode.SelectNodes("//li[@data-role='list-divider']"))
            {
                // gets date string from header segment
                string DayNodestr = DayNode.InnerText.Trim();
                string pattern = @"(\d{4}-\d{2}-\d{1,2}).*";
                Match m = Regex.Match(DayNodestr, pattern);
                string dateString = m.Value;
                DateOnly courseDate = DateOnly.Parse(dateString);

                //Console.WriteLine(">>>>>>>>>>>>>> DATE <<<<<<<<<<<<<<<");
                //Console.WriteLine($"{courseDate.ToString()}");
                //Console.WriteLine(">>>>>>>>>>>>>> DATE <<<<<<<<<<<<<<<");


                // Find courses under this date
                var nextSibling = DayNode.NextSibling;


                while (nextSibling != null && !nextSibling.GetAttributeValue("data-role", "").Equals("list-divider"))
                {
                    if (nextSibling.Name == "li")
                    {
                        if (nextSibling.SelectSingleNode(".//a") != null)
                        {
                            GetCoursesForecastResponse courseResponse = new GetCoursesForecastResponse();

                            string MoreInfoUrl = nextSibling.SelectSingleNode(".//a").GetAttributeValue("href", "");
                            
                            string journalRegexStr = @"journal_id=(\d+)";
                            string courseRegrxStr = @"course_id=(\d+)";
                            Regex journalRegex = new Regex(journalRegexStr);
                            Regex courseRegex = new Regex(courseRegrxStr);

                            string journalIdResult = journalRegex.Match(MoreInfoUrl).Groups[1].Value;
                            string courseIdResult = courseRegex.Match(MoreInfoUrl).Groups[1].Value;

                            int tempCourseId;
                            int tempJournalId;
                            Int32.TryParse(courseIdResult, out tempCourseId);
                            Int32.TryParse(journalIdResult, out tempJournalId);
                            courseResponse.CourseId = tempCourseId;
                            courseResponse.JournalId = tempJournalId;
                            string tagContent = nextSibling.SelectSingleNode(".//a").InnerText;

                            Regex trimmer = new Regex(@"[^\S\r\n]+");

                            tagContent = trimmer.Replace(tagContent, " ");


                            string[] contentArray = tagContent.Split("\n");
                            string[] resultContentArray = new string[5];

                            int resultContentArrayIteration = 0;
                            for(int i = 0; i < contentArray.Count(); i++)
                            { 
                                if((contentArray[i] != "" && contentArray[i] != " ") || contentArray[i] == "\n")
                                {
                                    resultContentArray[resultContentArrayIteration] = contentArray[i];
                                    resultContentArrayIteration++;
                                }
                            }

                            courseResponse.Title = resultContentArray[0];
                            courseResponse.PersonName = resultContentArray[1];
                            courseResponse.MeetingPoint = resultContentArray[2];
                            courseResponse.TimeSpan = resultContentArray[3];
                            courseResponse.CourseDate = courseDate;

                            methodResponse.Add(courseResponse);

                        }
                    }

                    nextSibling = nextSibling.NextSibling;
                }

            }

            return methodResponse;

        }

        public static GetCourseInfoResponse ConvertGetCourseDataResponse(string response)
        {
            // init classes
            GetCourseInfoResponse classResponse = new GetCourseInfoResponse();
            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(response);

            // get the div containing the spans
            HtmlNode? topDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='row row-cols-2']");

            // loop through all spans in html document
            List<string> contentList = new List<string>();
            int actualNodeIteration = 0;
            for(int x = 0; x < topDiv.ChildNodes.Count; x++)
            {
                HtmlNode curNode = topDiv.ChildNodes[x];

                // If current node is a span, add it to the content list
                if (curNode.Name == "span")
                {
                    contentList.Add(curNode.InnerText);
                }
            }

            // loop through the content list
            for(int y = 0; y < contentList.Count; y++)
            {
                // figure out what the current data is and add it to classResponse
                switch (contentList[y])
                {
                    case "Number of participants:":
                        classResponse.participantsCount = contentList[y + 1];
                        break;
                    case "Duration of course:":
                        classResponse.courseDuration = contentList[y + 1];
                        break;
                    case "From:":
                        classResponse.courseFromDateTime = contentList[y + 1];
                        break;
                    case "To:":
                        classResponse.courseToDateTime = contentList[y + 1];
                        break;
                    case "Course date:":
                        classResponse.courseDate = contentList[y + 1];
                        break;
                    case "Meeting point:":
                        classResponse.courseMeetingPoint = contentList[y + 1];
                        break;
                    case "Meeting time:":
                        classResponse.courseMeetingTime = contentList[y + 1];
                        break;
                    case "Course time:":
                        classResponse.courseTime = contentList[y + 1];
                        break;
                    case "Skill:":
                        classResponse.courseSkill = contentList[y + 1];
                        break;
                    case "Paid:":
                        classResponse.paidState = contentList[y + 1];
                        break;
                    case "State:":
                        classResponse.courseState = contentList[y + 1];
                        break;
                    case "Group type:":
                        classResponse.courseState = contentList[y + 1];
                        break;
                }
            }
           
            // return class response
            return classResponse;
        }

    }
}
