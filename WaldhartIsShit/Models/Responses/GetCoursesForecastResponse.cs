namespace WaldhartIsShit.Models.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetCoursesForecastResponse
    {
        public int CourseId { get; set; }

        public int JournalId { get; set; }

        public DateOnly CourseDate { get; set; }

        public string? Title { get; set; }

        public string? PersonName { get; set; }
        public string? TimeSpan { get; set; }
        public string? MeetingPoint { get; set; }

    }
}
