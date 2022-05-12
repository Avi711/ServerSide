using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models
{
    public class Chat
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string displayname { get; set; }

        [Required]
        public string server { get; set; }

        public List<Message> messages { get; set; }

    }
}
