using System;

namespace Forum.Models.Entities
{
    public abstract class EntityWithoutKey
    {
        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}