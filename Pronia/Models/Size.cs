﻿using System.ComponentModel.DataAnnotations;
using Pronia.Models.Base;



namespace Pronia.Models
{
    public class Size : BaseEntity
    {
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
