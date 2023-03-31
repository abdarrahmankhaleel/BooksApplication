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
    public class ServicesLogBook : IServesisRepositoryLog<LogBook>
    {
        private readonly FreeBookDbContext context;

        public ServicesLogBook(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid idT, Guid UserId)
        {
            try
            {
                LogBook logBook = new LogBook()
                {
                    Action = Helper.Delete,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BookId = idT,
                    Id = Guid.NewGuid(),
                };
                context.LogBooks.Add(logBook);
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
                if (cLogSubCategorieById == null)
                    return false;
                context.LogBooks.Remove(cLogSubCategorieById);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public LogBook FindBy(Guid id)
        {
            try
            {
                var LogSubCategorieById = context.LogBooks.Include(x => x.Book).FirstOrDefault(x => x.Id == id);
                return LogSubCategorieById;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<LogBook> GetAll()
        {
            try
            {
                var LogBooks = context.LogBooks.Include(x => x.Book).OrderByDescending(x => x.Date).ToList();
                return LogBooks;
            }
            catch (Exception)
            {
                return new List<LogBook>();
            }
        }

        public bool Save(Guid idT, Guid UserId)
        {
            try
            {
                LogBook LogBook = new LogBook()
                {
                    Action = Helper.Save,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BookId = idT,
                    Id = Guid.NewGuid(),
                };
                context.LogBooks.Add(LogBook);
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
                LogBook LogBook = new LogBook()
                {
                    Action = Helper.Update,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BookId = idT,
                    Id = Guid.NewGuid(),
                };
                context.LogBooks.Add(LogBook);
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
