using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Assistant.Models
{
   public class LoginU
    {
        [Key]
        public int LoginUId { get; set; }
        public string Login { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

     //   public User User { get; set; }

    }
}
