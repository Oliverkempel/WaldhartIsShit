namespace WaldhartIsShit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Course
    {

        public string? Title { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly startTime {  get; set; }
        public TimeOnly endTime { get; set; }
        public TimeSpan courseTimeSpan  
        { 
            get
            {
                //throw new NotImplementedException();
                return endTime - startTime;
                //return new TimeSpan();
            }
        }


        //public string? GroupType { get; set; }
        //public string? GroupSkillLevel { get; set; }
        //public string? MeetingPoint { get; set; }
        //public int ParticipantsAmount { get; set; }
        //public int CourseDuration { get; set; }
        //public TimeOnly MeetingTime { get; set; }
        //public TimeOnly EndTime { get; set; }
        //public DateTime CourseFromDate { get; set; }
        //public DateTime CourseToDate { get; set; }
        //public string? Notes { get; set; }
        //public string? Status { get; set; }
    }
}
