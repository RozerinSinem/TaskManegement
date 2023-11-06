using System.ComponentModel.DataAnnotations;

namespace TaskManegementSystem.Models
{
    public class TaskViewModel
    {
        [Key]
        public int TaskID { get; set; }
        
        [Required]
        public String Name { get; set; }
       
        [Required]
        public String Description { get; set; }

        public String? AssignedUser { get; set; }

    }
}
