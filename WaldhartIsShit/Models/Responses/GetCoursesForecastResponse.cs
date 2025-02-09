namespace WaldhartIsShit.Models.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class GetCoursesForecastResponse
    {
        public int CourseId { get; set; }

        public int JournalId { get; set; }

        public string CourseDate { get; set; }

        public DateOnly CourseDate_Converted 
        { get 
            {
                DateOnly courseDate = new DateOnly();
                if (!String.IsNullOrEmpty(CourseDate))
                {
                    string pattern = @"(\d{4}-\d{2}-\d{1,2}).*";
                    Match m = Regex.Match(CourseDate, pattern);
                    string dateString = m.Value;
                    courseDate = DateOnly.Parse(dateString);
                }

                return courseDate;
            } 
        }

        public string? Data01 { get; set; }
        public string? Data02 { get; set; }
        public string? Data03 { get; set; }
        public string? Data04 { get; set; }

    }
}
