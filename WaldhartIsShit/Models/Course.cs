namespace WaldhartIsShit.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommunityToolkit;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class Course : ObservableObject
    {
        public Course()
        {
            courseTimeSpan = endTime - startTime;
        }

        public string? Title { get; set; }
        public DateOnly Date { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(courseTimeSpan))]
        public TimeOnly startTime;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(courseTimeSpan))]
        public TimeOnly endTime;

        //partial void OnStartTimeChanged(TimeOnly oldValue, TimeOnly newValue)
        //{
        //    courseTimeSpan = endTime - startTime;
        //}

        //partial void OnEndTimeChanged(TimeOnly oldValue, TimeOnly newValue)
        //{
        //    courseTimeSpan = endTime - startTime;

        //}


        //[ObservableProperty]
        public TimeSpan _courseTimeSpan;

        public TimeSpan courseTimeSpan
        {
            get
            {
                if(EndTime != TimeOnly.MinValue || StartTime != TimeOnly.MinValue)
                {
                    _courseTimeSpan = endTime - startTime;
                }
                return _courseTimeSpan;
            }
            set
            {
                StartTime = TimeOnly.MinValue;
                EndTime = TimeOnly.MinValue;
                _courseTimeSpan = value;
                OnPropertyChanged(nameof(courseTimeSpan));


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
