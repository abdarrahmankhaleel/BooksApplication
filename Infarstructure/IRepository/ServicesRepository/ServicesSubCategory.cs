using Domin.Entity;
using Infarstructure.IRepository;
using Infarstuructre.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.IRepository.ServicesRepository
{
    public class ServicesSubCategory : IServicesRepository<SubCategory>
    {
        private readonly FreeBookDbContext context;

        public ServicesSubCategory(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid id)
        {
            try
            {
                var subCategory=context.SubCategories.FirstOrDefault(x => x.Id == id);
                if (subCategory != null)
                {
                    subCategory.CurrentState = 0;
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public SubCategory FindBy(Guid id)
        {
            try
            {

                var subCategory=context.SubCategories.Include(sb => sb.Category).FirstOrDefault(sb => sb.Id == id && sb.CurrentState > 0);
                if(subCategory != null)
                    return subCategory;
                else
                    return new SubCategory();
            }
            catch (Exception)
            {

                return new SubCategory();
            }
        }
        public SubCategory FindBy(string name)
        {
            try
            {
                return context.SubCategories.FirstOrDefault(sb => sb.Name == name.Trim() && sb.CurrentState > 0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SubCategory> GetAll()
        {
            try
            {
            var subCategoriesList=context.SubCategories.Include(sc => sc.Category).OrderBy(s => s.Name).Where(x => x.CurrentState > 0).ToList();
            return subCategoriesList;

            }
            catch (Exception)
            {
                return new List<SubCategory>();
                
            }
        }

        public List<SubCategory> GetSubcategoriesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public bool Save(SubCategory model)
        {
            try
            {
                if (model.Id == Guid.Empty)//Add
                {
                    model.Id = Guid.NewGuid();
                    model.CurrentState = 1;
                    context.SubCategories.Add(model);

                }
                else//Edit
                {
                    model.CurrentState++;
                    context.SubCategories.Update(model);
                }
                 
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

              return  false;
            }
        }
    }
}
