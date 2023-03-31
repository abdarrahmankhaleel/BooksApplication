using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstructure.IRepository
{
    public interface IServesisRepositoryLog<T> where T : class
    {
        List<T> GetAll();
        T FindBy(Guid id);
        bool Save(Guid idT,Guid UserId);
        bool Update(Guid idT, Guid UserId);
        bool Delete(Guid idT,Guid UserId);
        bool DeleteLog(Guid Id);
    }
}
