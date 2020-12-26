using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Student_Assistant.Models
{
   public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public int NumParCH { get; set; }
        public int DayCH { get; set; }
        public int NumParZN { get; set; }
        public int DayZN { get; set; }
    }
}
