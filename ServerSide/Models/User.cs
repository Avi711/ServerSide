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

        public string DisplayName { get; set; }

        public string Image { get; set; }

        public List<Chat> Chats { get; set; }
    }
}
