using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Assistant.Models
{
  public  class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        public string Name { get; set; }

        public double Mark { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime Half { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public int HalfWork { get; set; }
        public int? UserId { get; set; }
        public User  User { get; set; }

    }
}
