using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        [Display(Name ="Published Date")]
        public String PublishedDate { get; set; }

        public string Name { get; set; }


        [Required]
        [Range(1, 5)]
        [Display(Name = "Rate")]
        public int rate { get; set; }

    }
}
