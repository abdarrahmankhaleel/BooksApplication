using Domin.Entity;
using Infarstuructre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstructure.IRepository.ServicesRepository
{
    public class ServicesCategory : IServicesRepository<Category>
    {
        private readonly FreeBookDbContext context;

        public ServicesCategory(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid id)
        {
            try
            {
                var category=FindBy(id);
                category.CurrentState =(int)Helper.eCurrentState.Delete;
                context.Categories.Update(category);
                context.SaveChanges();  
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Category> GetAll()
        {
            try
            {
                return context.Categories.OrderBy(x => x.Name).Where(x=>x.CurrentState>0).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public Category FindBy(Guid id)
        {
            try
            {
                return context.Categories.FirstOrDefault(x=>x.Id.Equals(id)&&x.CurrentState>0);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Category model)
        {
            try
            {
                var Category = FindBy(model.Id);
                if(Category==null)
                {
                    model.Id = Guid.NewGuid();
                    model.CurrentState = (int)Helper.eCurrentState.Active;
                    context.Categories.Add(model);
                }
                else
                {
                    Category.Description = model.Description;
                    Category.Name = model.Name;
                    Category.CurrentState=(int)Helper.eCurrentState.Active;
                    context.Categories.Update(Category);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Category FindBy(string name)
        {
            try
            {
                return context.Categories.FirstOrDefault(x => x.Name.Equals(name.Trim())&&x.CurrentState>0);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<SubCategory> GetSubcategoriesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
