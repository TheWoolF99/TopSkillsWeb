using Core.Account;
using Data.Repository;
using Interfaces.Users;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class UserRepository : IRepository<User>
    {
        private DbContextFactory _context;

        public UserRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public IQueryable<User> GetAll()
        {
            return new List<User>()
            {
                new(){ Id ="Test", UserName = "TETST"},
                new(){ Id ="Test2", UserName = "TETST2"},
                new(){ Id ="Test3", UserName = "TETST3"}
            }.AsQueryable();
        }


        public async Task<User> GetAsync(string id)
        {
            return new User{
                Id = "1", UserName="Stta"
            };
        }

    }
}
