using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Context.Entities.Common
{
    /// <summary>
    /// Represents the base entity with a unique identifier.
    /// All derived entities inherit this unique identifier property.
    /// </summary>
    [Index("Uid", IsUnique = true)]
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// This value is generated automatically.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Uid { get; set; } = Guid.NewGuid();
    }
}
