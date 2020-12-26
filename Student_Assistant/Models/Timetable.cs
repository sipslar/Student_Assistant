using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Assistant.Models
{
    public class Timetable
    {
        [Key]
        public int TimetableId { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
