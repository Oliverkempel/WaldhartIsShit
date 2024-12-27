namespace WaldhartIsShit
{
    using RestSharp;

    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public string getCoursePath { get; set; } = "mycourses/course/get_course_data?journal_id=&course_id=%1&course_day_iso=%2";
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

        public void getCoursesForDateRange()
        {
            RestRequest request = new RestRequest();
        }
        public void getCourse()
        {

        }

        public Uri CreateGetCoursesUri(DateOnly fromDate, DateOnly toDate)
        {
            return new Uri(WaldhartUrl, getCoursesPath.Replace("%1", fromDate.ToString(DateOnlyFormat)).Replace("%2", toDate.ToString(DateOnlyFormat)));
        }

        public Uri CreateGetCourseUri(int courseId, DateOnly courseDate)
        {
            string test = getCoursePath.Replace("%1", courseId.ToString()).Replace("%2", courseDate.ToString(DateOnlyFormat));
            return new Uri(WaldhartUrl, getCoursePath.Replace("%1", courseId.ToString()).Replace("%2", courseDate.ToString(DateOnlyFormat)));
        }
    }
}
