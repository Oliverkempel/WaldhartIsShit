namespace WaldhartIsShit
{
    using RestSharp;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Intrinsics.Arm;
    using System.Text;
    using System.Threading.Tasks;

    public class WaldhartRequestHandler
    {
        // Parameterize me
        public readonly string DateOnlyFormat = "yyyy-MM-dd";
        public Uri WaldhartUrl { get; set; } = new Uri("https://kvitfjell-desk.skischoolshop.com/");

        // Parameterize me
        public string getCoursesPath { get; set; } = "mycourses/get_courses_forecast_list?from_date_iso=%1&to_date_iso=%2";
        // Parameterize me
        public string getCoursePath { get; set; } = "mycourses/course/get_course_data?journal_id=%1&course_id=%2&course_day_iso=%3";
        // Parameterize me
        public string getSessionIdPath { get; set; } = "login?username=%1&password=%2";


        public string sessionCookie { get; set; } = "8d1eb25817e5f0bb488168d9154ce1533305fe3f";

        public RestClient client { get; set; }
        public RestClientOptions options { get; set; }
        public WaldhartRequestHandler() 
        {
            options = new RestClientOptions
            {
                Timeout = new TimeSpan(0, 0, 5),
                BaseUrl = WaldhartUrl,
            };
            client = new RestClient(options);
        }
        /// <summary>
        /// Gets the session id for waldhart.
        /// </summary>
        /// <param name="username">The username for your waldhart account. eg. JhonDoe12</param>
        /// <param name="password">The password for your waldhart account. eg. 1234</param>
        public string? login(string username, string password)
        {
            string? sessionId = getSessionId(username, password).Cookies?.Where(cookie => cookie.Name == "session_id")?.FirstOrDefault()?.Value;
            sessionCookie = sessionId;

            return sessionId;
        }

        /// <summary>
        /// Fetches Courses for a specified date range.
        /// </summary>
        /// <param name="fromDate">The from date in ISO format. yyyy-MM-dd</param>
        /// <param name="toDate">The to date in ISO format. yyyy-MM-dd</param>
        /// <returns></returns>
        public string fetchCoursesFromDateRange(DateOnly fromDate, DateOnly toDate)
        {
            RestResponse response = getCoursesForecast(fromDate, toDate);
            string? responseContent = response.Content;

            // DO CONVERSION HERE!!!!

            return responseContent;
        }

        /// <summary>
        /// Fetches data from a specific course.
        /// </summary>
        /// <param name="journalId">The journal id of the course, can be left empty</param>
        /// <param name="courseId">The course id</param>
        /// <param name="date">the date of the course</param>
        /// <returns></returns>
        public string fetchCourseData(int journalId, int courseId, DateOnly date)
        {
            RestResponse response = getCourseData(journalId, courseId, date);
            string? responseContent = response.Content;

            return responseContent;
        }

        private RestResponse getSessionId(string userId, string userPass)
        {
            RestRequest request = new RestRequest(CreateGetSessionIdUri(userId, userPass));
            request.AddCookie("session_id", $"{sessionCookie}", "/", "kvitfjell-desk.skischoolshop.com");
            request.AddCookie("L", "en", "/", "kvitfjell-desk.skischoolshop.com");

            RestResponse response = client.Execute(request);

            return response;
        }

        private RestResponse getCoursesForecast(DateOnly fromDate, DateOnly toDate)
        {
            RestRequest request = new RestRequest(CreateGetCoursesUri(fromDate, toDate).ToString(), Method.Get);
            request.AddCookie("session_id", $"{sessionCookie}", "/", "kvitfjell-desk.skischoolshop.com");
            request.AddCookie("L", "en", "/", "kvitfjell-desk.skischoolshop.com");

            RestResponse? response = client.Execute(request);

            return response;
        }
        private RestResponse getCourseData(int journalId, int courseId, DateOnly courseDate)
        {
            RestRequest request = new RestRequest(CreateGetCourseUri(journalId, courseId, courseDate).ToString(), Method.Get);
            request.AddCookie("session_id", $"{sessionCookie}", "/", "kvitfjell-desk.skischoolshop.com");
            request.AddCookie("L", "en", "/", "kvitfjell-desk.skischoolshop.com");

            RestResponse? response = client.Execute(request);

            return response;
        }

        private Uri CreateGetCoursesUri(DateOnly fromDate, DateOnly toDate)
        {
            return new Uri(WaldhartUrl, getCoursesPath.Replace("%1", fromDate.ToString(DateOnlyFormat)).Replace("%2", toDate.ToString(DateOnlyFormat)));
        }

        private Uri CreateGetSessionIdUri(string userId, string userPass)
        {
            return new Uri(WaldhartUrl, getSessionIdPath.Replace("%1", userId).Replace("%2", userPass));
        }

        private Uri CreateGetCourseUri(int journalId, int courseId, DateOnly courseDate)
        {
            string test = getCoursePath.Replace("%1", journalId.ToString()).Replace("%2", courseId.ToString()).Replace("%3", courseDate.ToString(DateOnlyFormat));
            return new Uri(WaldhartUrl, getCoursePath.Replace("%1", journalId.ToString()).Replace("%2", courseId.ToString()).Replace("%3", courseDate.ToString(DateOnlyFormat)));
        }
    }
}
