﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models
{
    public class Contact
    {
   
        public string id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string server { get; set; }

        public string last { get; set; }

        public DateTime lastdate { get; set; }

    }
}
