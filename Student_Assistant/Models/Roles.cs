using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Assistant.Models
{
   public class Roles
    {
        [Key]
        public int RolesId { get; set; }
        public string Name { get; set; }

        public ICollection<User>  Users { get; set; }
    }
}
