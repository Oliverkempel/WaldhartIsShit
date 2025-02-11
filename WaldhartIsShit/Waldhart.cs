namespace WaldhartIsShit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using HtmlAgilityPack;

    using WaldhartIsShit.Models;
    using WaldhartIsShit.Models.Responses;

    public class Waldhart
    {
        public WaldhartRequestHandler RequestHandler { get; set; }
        public ObservableCollection<Course> Courses { get; set;} = new ObservableCollection<Course>();

        public List<string> excludedCourseNames { get; set;} = new List<string>();

        public Waldhart()
        {
            RequestHandler = new WaldhartRequestHandler();
        }

        public void login(string username, string password)
        {
            RequestHandler.login(username, password);
        }

        public List<Course> fetchCourses(DateOnly fromDate, DateOnly toDate)
        {
            // Gets Courses forecast from date range & converts to useable object
            string? result = RequestHandler.fetchCoursesFromDateRange(fromDate, toDate);
            List<GetCoursesForecastResponse> resp = ConvertGetCoursesForecastResponse(result);

            // Inits list of course info response objects
            List<GetCourseInfoResponse> courseDataResps = new List<GetCourseInfoResponse>();

            // Inits list of courseses objects (final datatype)
            //List<Course> courses = new List<Course>();

            // Loops though objects generated from Courses forecast call
            foreach (var curResp in resp)
            {

                // Inits GetCourseInfoResponse object for storeing data.
                GetCourseInfoResponse courseDataResp = new GetCourseInfoResponse();

                // Checks if current forecast item (course) has either Course id or Journal id
                if (curResp.CourseId != 0 || curResp.JournalId != 0)
                {
                    // If current forecast item (course) has either courseId or Journal Id fetch extra data using fetchCourseData endpoint
                    string dataResponse = RequestHandler.fetchCourseData(curResp.JournalId, curResp.CourseId, curResp.CourseDate_Converted);

                    // Deserialize raw html response to GetCourseInfoResponse Object
                    courseDataResp = ConvertGetCourseDataResponse(dataResponse);

                    // Set title & courseDate from data fetched from course forecast, as this is not found in fetchCourseData endpoint
                    courseDataResp.Title = curResp.Data01;
                    courseDataResp.courseDate = curResp.CourseDate;
                }
                else
                {
                    // If no journalId or CourseId exists, build GetCourseInfoResponse from forecast data instead
                    courseDataResp.courseDate = curResp.CourseDate.ToString();
                    courseDataResp.Title = curResp.Data01;
                    courseDataResp.courseTime = curResp.Data04;
                }

                // Add current GetCourseInfoResponse object to list
                courseDataResps.Add(courseDataResp);


                // Convert courseTime property to starttime and endtime
                string[] splittTimeSpanString = courseDataResp.courseTime.Split("-");
                string fromStr = splittTimeSpanString[0];
                string toStr = splittTimeSpanString[1];

                TimeOnly startTimeConverted = TimeOnly.Parse(fromStr.Trim());
                TimeOnly endTimeConverted = TimeOnly.Parse(toStr.Trim());


                // Check if course is excluded
                if (!excludedCourseNames.Any(courseDataResp.Title.Contains))
                {
                    // Create, populate and add the final course object to the Courses list
                    Courses.Add(new Course { Title = courseDataResp.Title, Date = curResp.CourseDate_Converted, startTime = startTimeConverted, endTime = endTimeConverted });
                }
            }

            return Courses.ToList();
        }

        public TimeSpan summarizeHours(DateOnly fromDate, DateOnly toDate)
        {
            List<Course> result = Courses.Where(x => x.Date >= fromDate && x.Date <= toDate).ToList();
            
            return new TimeSpan(result.Sum(x => x.courseTimeSpan.Ticks));
        }

        public static List<GetCoursesForecastResponse> ConvertGetCoursesForecastResponse(string response)
        {
            List<GetCoursesForecastResponse> methodResponse = new List<GetCoursesForecastResponse>();
            HtmlDocument? HtmlDoc = new HtmlDocument();

            HtmlDoc.LoadHtml(response);

            // Handle null exception for below code
            foreach (var DayNode in HtmlDoc.DocumentNode.SelectNodes("//li[@data-role='list-divider']"))
            {
                // gets date string from header segment
                string DayNodestr = DayNode.InnerText.Trim();
                string pattern = @"(\d{4}-\d{2}-\d{1,2}).*";
                Match m = Regex.Match(DayNodestr, pattern);
                DayNodestr = m.Value;
                //DateOnly courseDate = DateOnly.Parse(dateString);

                // Find Courses under this date
                HtmlNode? nextSibling = DayNode.NextSibling;

                while (nextSibling != null && !nextSibling.GetAttributeValue("data-role", "").Equals("list-divider"))
                {
                    if (nextSibling.Name == "li")
                    {
                        if (nextSibling.SelectSingleNode(".//a") != null)
                        {
                            GetCoursesForecastResponse courseResponse = new GetCoursesForecastResponse();

                            HtmlNode? aNode = nextSibling.SelectSingleNode(".//a[@class='main_link']");

                            if(aNode != null)
                            {
                                string MoreInfoUrl = aNode.GetAttributeValue("href", "");

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

                                // Below is goofy, dont assume lines have meaning, the only importent data in this step is: JournalID, CourseID and CourseTime

                                string tagContent = nextSibling.SelectSingleNode(".//a").InnerText;

                                Regex trimmer = new Regex(@"[^\S\r\n]+");

                                tagContent = trimmer.Replace(tagContent, " ");


                                string[] contentArray = tagContent.Split("\n");
                                string[] resultContentArray = new string[5];

                                int resultContentArrayIteration = 0;
                                for (int i = 0; i < contentArray.Count(); i++)
                                {
                                    if ((contentArray[i] != "" && contentArray[i] != " ") || contentArray[i] == "\n")
                                    {
                                        resultContentArray[resultContentArrayIteration] = contentArray[i];
                                        resultContentArrayIteration++;
                                    }
                                }

                                courseResponse.Data01 = resultContentArray[0];
                                courseResponse.Data02 = resultContentArray[1];
                                courseResponse.Data03 = resultContentArray[2];
                                courseResponse.Data04 = resultContentArray[3];

                                courseResponse.CourseDate = DayNodestr;
                            } else
                            {
                                aNode = nextSibling.SelectSingleNode(".//a[@data-rel='popup']");

                                string tagContent = aNode.SelectSingleNode(".//span").InnerHtml;

                                Regex trimmer = new Regex(@"[^\S\r\n]+");

                                tagContent = trimmer.Replace(tagContent, " ");

                                string[] contentArray = tagContent.Split("\n");
                                string[] resultContentArray = new string[2];

                                int resultContentArrayIteration = 0;
                                for (int i = 0; i < contentArray.Count(); i++)
                                {
                                    if ((contentArray[i] != "" && contentArray[i] != " " && !contentArray[i].Contains("<br>")) || contentArray[i] == "\n")
                                    {
                                        resultContentArray[resultContentArrayIteration] = contentArray[i];
                                        resultContentArrayIteration++;
                                    }
                                }

                                courseResponse.Data01 = resultContentArray[0];
                                courseResponse.CourseDate = DayNodestr;
                                courseResponse.Data04 = resultContentArray[1];

                            }

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
