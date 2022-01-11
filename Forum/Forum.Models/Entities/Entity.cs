using Forum.Models.Entities.Contracts;

using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Entities
{
    public class Entity : IEntity
    {
        [Key]
        public long Id { get ; set ; }

        public DateTime CreatedOn { get ; set ; }

        public DateTime ModifiedOn { get ; set ; }

        public bool IsDeleted { get ; set ; }

        public DateTime? DeletedOn { get ; set ; }
    }
}