namespace WaldhartIsShit.Models.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class GetCourseInfoResponse
    {
        public string Title { get; set; }
        public string participantsCount { get; set; }
        public string courseDuration { get; set; }
        public string courseFromDateTime { get; set; }
        public string courseToDateTime { get; set; }
        public string courseMeetingPoint { get; set; }
        public string courseMeetingTime { get; set; }
        public string courseTime { get; set; }
        public string courseSkill { get; set; }
        public string paidState { get; set; }
        public string courseState { get; set; }

        public string courseDate { get; set; }

        public DateOnly courseDate_Converted
        {
            get
            {
                return convertWaldhartDateStr(courseDate);
            }
        }

        public DateOnly courseFromDateTime_Converted
        {
            get
            {
                return convertWaldhartDateStr(courseFromDateTime);
            }
        }

        public DateOnly courseToDateTime_Converted
        {
            get
            {
                return convertWaldhartDateStr(courseToDateTime);
            }
        }

        public DateOnly convertWaldhartDateStr(string input)
        {
            DateOnly converted_courseDate = new DateOnly();
            if (!string.IsNullOrEmpty(input))
            {
                string pattern = @"(\d{4}-\d{2}-\d{1,2}).*";
                Match m = Regex.Match(input, pattern);
                string dateString = m.Value;
                converted_courseDate = DateOnly.Parse(dateString);
            }

            return converted_courseDate;
        }
    }
}
