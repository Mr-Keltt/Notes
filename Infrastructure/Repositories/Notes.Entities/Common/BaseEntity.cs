﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Context.Entities.Common;

[Index("Uid", IsUnique = true)]
public abstract class BaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    [Required]
    public virtual Guid Uid { get; set; } = Guid.NewGuid();
}
