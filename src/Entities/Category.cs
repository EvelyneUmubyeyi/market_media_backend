﻿using System.ComponentModel.DataAnnotations;

namespace MarketMedia.src.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public byte[] CategoryImg { get; set; }
    }
}
