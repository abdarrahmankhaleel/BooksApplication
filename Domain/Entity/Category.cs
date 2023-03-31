using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domin;
namespace Domin.Entity
{
   public class Category
    {
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType =typeof(ResourceData),ErrorMessageResourceName ="CtaegoryName")]
        [MaxLength(20,ErrorMessageResourceType =typeof(ResourceData),ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLength")]
        public string Name { get; set; }
        public string Description { get; set; }

        public int CurrentState { get; set; }
    }
}
