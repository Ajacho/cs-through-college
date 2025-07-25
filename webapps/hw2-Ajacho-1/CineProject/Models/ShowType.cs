﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CineProject.Models;

[Table("ShowType")]
public partial class ShowType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(20)]
    public string ShowTypeIdentifier { get; set; } = null!;

    [InverseProperty("ShowType")]
    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
