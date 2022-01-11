using System.Collections.Generic;

namespace Forum.Models.Entities
{
    public class Role : Entity
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
        public string Type { get; set; }
        public ICollection<User> Users { get; set; }
        
    }
}