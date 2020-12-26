using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Assistant.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }

        public string FName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Log { get; set; }
        public string Grup { get; set; }
        public int? LoginUId { get; set; }
        public LoginU LoginU { get; set; }

        public Roles Roles { get; set; }
    }
}
