using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Image { get; set; }

        public List<contact> contacts { get; set; }
    }
}
