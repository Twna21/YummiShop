using BusinessObject;
using DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task Add(Account pro) => await AccountDAO.Instance.Add(pro);


        public async Task Delete(Account pro) => AccountDAO.Instance.Delete(pro);


        public async Task<Account> GetAccountById(int id) => await AccountDAO.Instance.GetAccountById(id);


        public async Task<IEnumerable<Account>> GetAccounts() => await AccountDAO.Instance.GetAccounts();


        public async Task Update(Account pro) => await AccountDAO.Instance.Update(pro);
        public async Task GetAccountByEmail(string pro) => await AccountDAO.Instance.GetAccountByEmail(pro);
    }
}
