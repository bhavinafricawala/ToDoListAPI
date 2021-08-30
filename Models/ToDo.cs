using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models
{
    public class ToDo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string ToDoText { get; set; }

        [Required]
        public User user { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
    }
}