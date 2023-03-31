using Domin.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class ViewModelBooks
    {
        public List<Book> LstBooks { get; set; }=new List<Book>();
        public List<LogBook> LstLogBooks { get; set; }= new List<LogBook>();
        public Book Book { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}
