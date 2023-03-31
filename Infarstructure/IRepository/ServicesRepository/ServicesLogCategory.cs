using Domin.Entity;
using Infarstuructre.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstructure.IRepository.ServicesRepository
{
    public class ServicesLogCategory : IServesisRepositoryLog<LogCategory>
    {
        private readonly FreeBookDbContext context;

        public ServicesLogCategory(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid idT, Guid UserId)
        {
            try
            {
                var logCategory = new LogCategory()
                {
                    Id=Guid.NewGuid(),  
                    Action=Helper.Delete,
                    UserId=UserId,
                    Date=DateTime.Now,
                    CategoryId=idT,
                };
                context.LogCategories.Add(logCategory);
                context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool DeleteLog(Guid Id)
        {
            try
            {
                var logCategory=FindBy(Id);
                context.LogCategories.Remove(logCategory);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<LogCategory> GetAll()
        {
            try
            {
                
                return context.LogCategories.Include(x=>x.Category).OrderByDescending(x=>x.Date).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public LogCategory FindBy(Guid id)
        {
            try
            {
                var result=context.LogCategories.Include(x => x.Category).FirstOrDefault(x=>x.Id==id);  
                return result;
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Save(Guid idT, Guid UserId)
        {
            try
            {
                var logCategory = new LogCategory()
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Save,
                    UserId = UserId,
                    Date = DateTime.Now,
                    CategoryId = idT,
                };
                context.LogCategories.Add(logCategory);
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
                var logCategory = new LogCategory()
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Update,
                    UserId = UserId,
                    Date = DateTime.Now,
                    CategoryId = idT,
                };
                context.LogCategories.Add(logCategory);
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
