﻿using System.ComponentModel.DataAnnotations;

namespace CrimeCode.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public Tag() { }
    }
}
