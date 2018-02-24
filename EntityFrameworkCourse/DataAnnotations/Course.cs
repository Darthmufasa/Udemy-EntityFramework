namespace DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    public partial class Course
    {
        public Course()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        public int Level { get; set; }

        public float FullPrice { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
