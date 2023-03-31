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
    public class ServicesLogSubCategory : IServesisRepositoryLog<LogSubCategory>
    {
        private readonly FreeBookDbContext context;

        public ServicesLogSubCategory(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid idT, Guid UserId)
        {
            try
            {
                LogSubCategory logSubCategory = new LogSubCategory()
                {
                    Action=Helper.Delete,
                    Date=DateTime.Now,
                    UserId=UserId,
                    SubCategoryId=idT,
                    Id=Guid.NewGuid(),  
                };
                context.LogSubCategories.Add(logSubCategory);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteLog(Guid Id)
        {
            try
            {
                var cLogSubCategorieById = FindBy(Id);
                if(cLogSubCategorieById == null)
                    return false;
                context.LogSubCategories.Remove(cLogSubCategorieById);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public LogSubCategory FindBy(Guid id)
        {
            try
            {
                var LogSubCategorieById = context.LogSubCategories.Include(x => x.SubCategory).FirstOrDefault(x=>x.Id==id);
                return LogSubCategorieById;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<LogSubCategory> GetAll()
        {
            try
            {
                var LogSubCategories = context.LogSubCategories.Include(x => x.SubCategory).OrderByDescending(x=>x.Date).ToList();
                return LogSubCategories;
            }
            catch (Exception)
            {
                return new List<LogSubCategory>() ;
            }
        }

        public bool Save(Guid idT, Guid UserId)
        {
            try
            {
                LogSubCategory logSubCategory = new LogSubCategory()
                {
                    Action = Helper.Save,
                    Date = DateTime.Now,
                    UserId = UserId,
                    SubCategoryId = idT,
                    Id = Guid.NewGuid(),
                };
                context.LogSubCategories.Add(logSubCategory);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public bool Update(Guid idT, Guid UserId)
        {
            try
            {
                LogSubCategory logSubCategory = new LogSubCategory()
                {
                    Action = Helper.Update  ,
                    Date = DateTime.Now,
                    UserId = UserId,
                    SubCategoryId = idT,
                    Id = Guid.NewGuid(),
                };
                context.LogSubCategories.Add(logSubCategory);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
