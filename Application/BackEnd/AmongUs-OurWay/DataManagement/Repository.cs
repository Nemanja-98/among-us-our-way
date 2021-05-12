using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.DataManagement
{
    public class Repository : IRepository
    {
        private readonly AmongUsContext _dbContext;
        public Repository(RepositoryConnection con)
        {
            _dbContext = con.Context;
        }
        public IGameRepository GetGameRepository()
        {
            return new GameRepository(_dbContext);
        }

        public IUserRepository GetUserRepository()
        {
            return new UserRepository(_dbContext);
        }
    }
}