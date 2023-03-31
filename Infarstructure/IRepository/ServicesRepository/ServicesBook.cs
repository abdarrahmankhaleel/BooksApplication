using Domin.Entity;
using Infarstructure.IRepository;
using Infarstuructre.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository.ServicesRepository
{
    public class ServicesBook : IServicesRepository<Book>
    {
        private readonly FreeBookDbContext context;

        public ServicesBook(FreeBookDbContext context)
        {
            this.context = context;
        }
        public bool Delete(Guid id)
        {
            try
            {
                var book = FindBy(id);
                book.CurrentState = 0;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Book FindBy(Guid id)
        {
            try
            {
            var book = context.Books.Include(b => b.Category)
                .Include(b => b.SubCategory).
                FirstOrDefault(b => b.CurrentState > 0 && b.Id == id );
                if (book == null)
                    return new Book();
                return book;

            }
            catch (Exception)
            {
                return new Book();
            }
        }

        public Book FindBy(string name)
        {
            try
            {
                var BookSameNaame = context.Books.FirstOrDefault(B =>B.CurrentState>0&& B.Name == name);
                return BookSameNaame == null ? null : BookSameNaame;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Book> GetAll()
        {
            try
            {
                List<Book> books = context.Books.Include(b => b.Category)
                .Include(b => b.SubCategory).
                Where(b => b.CurrentState > 0).ToList();
                return books;
            }
            catch (Exception)
            {
                return new List<Book>();
            }

        }

        public List<SubCategory> GetSubcategoriesByCategoryId(Guid categoryId)
        {
            return context.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .ToList();
        }

        public bool Save(Book model)
        {
            try
            {
                var Book = FindBy(model.Id);
                if (model.Id == Guid.Empty)//add
                {
                    model.Id = Guid.NewGuid();
                    model.CurrentState = (int)Helper.eCurrentState.Active;
                    context.Books.Add(model);
                }
                else//edit
                {
                    Book.Name= model.Name;  
                    Book.Author= model.Author;  
                    Book.CategoryId= model.CategoryId;
                    Book.SubCategoryId= model.SubCategoryId;
                    Book.Description= model.Description;
                    Book.CurrentState=(int)Helper.eCurrentState.Active;
                    context.Books.Update(model);
                }
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
