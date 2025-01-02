namespace WaldhartIsShit.Models.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetCourseInfoResponse
    {
        public string participantsCount { get; set; }

        public string courseDuration { get; set; }

        public string courseDate { get; set; }
        public string courseFromDateTime { get; set; }
        public string courseToDateTime { get; set; }

        public string courseMeetingPoint { get; set; }

        public string courseMeetingTime { get; set; }

        public string courseTime { get; set; }

        public string courseSkill { get; set; }

        public string paidState { get; set; }

        public string courseState { get; set; }
    }
}
