using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstructure.IRepository
{
    //التي دي عشان الانترفيس هتستقبل اي كلاس مودل فبتبقى الانترفيس جنيرك
     // <T> is place to you can post any entity or class model like categoy,book ,subcategory esc..

    public interface IServicesRepository<T> where T : class
    {
        List<T> GetAll();
        T FindBy(Guid id);
        T FindBy(string name);
        bool Save(T model);
        bool Delete(Guid id);


        List<SubCategory> GetSubcategoriesByCategoryId(Guid categoryId);

    }
}
