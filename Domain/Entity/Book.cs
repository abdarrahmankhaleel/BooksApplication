﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domin.Entity
{
   public class Book
    {
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "BookName")]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLength")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "AuthorName")]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLength")]
        public string Author { get; set; }

        public string? ImageName { get; set; }
        public string? FileName { get; set; }

        public string Description { get; set; }

        public bool? Publish { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public SubCategory SubCategory { get; set; }

        public int CurrentState { get; set; } = 1;

    }
}
