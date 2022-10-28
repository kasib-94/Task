using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3,ErrorMessage = "Title between 3-50 letters")  ]
        public string Title { get; set; }
        [StringLength(50, MinimumLength = 3,ErrorMessage = "Author Name between 3-50 letters")  ]
        public string Author { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        
        public string ReleaseDateString { get; set; }
        [StringLength(400, MinimumLength = 10,ErrorMessage = "Description between 10-400 letters")  ]
        public string Description { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; }
    }
}