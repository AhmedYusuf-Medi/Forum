using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Entities
{
    public class Category : Entity
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "Category must be between {2} and {1}")]
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}