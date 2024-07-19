using CreditCards.APITest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCards.APITest.Services
{
    public interface IDepartment
    {
        Task<IEnumerable<Department>> GetAll();
        Task<Department> GetById(string id);
    }
}
