using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class ViewModelSubCategory
    {
        public List<SubCategory> lstSubCategories { get; set; }=new List<SubCategory>();
        public List<LogSubCategory> lstLogSubCategories { get; set; } = new List<LogSubCategory>(); 
        public SubCategory SubCategory { get; set; } = new SubCategory();
    }
}
