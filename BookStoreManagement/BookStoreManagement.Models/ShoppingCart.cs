﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreManagement.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
