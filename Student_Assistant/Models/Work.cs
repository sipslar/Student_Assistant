using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Student_Assistant.Models
{
    public class Work
    {
        [Key]
        public int WorkId { get; set; }
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public double Mark { get; set; }
        public double MarkMax { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public bool Passed { get; set; }
    }
}
