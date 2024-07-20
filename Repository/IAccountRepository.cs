using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccountById(int id);
        Task Add(Account pro);
        Task Update(Account pro);
        Task Delete(Account pro);
        Task GetAccountByEmail(string pro);
    }
}
