﻿namespace WaldhartIsShit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DayCourse
    {
        DateOnly CourseDate { get; set; }
        public Course? Course { get; set; }
    }
}
