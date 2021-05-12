using Microsoft.EntityFrameworkCore;
using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.DataManagement
{
    public class RepositoryConnection
    {
        private readonly AmongUsContext _dbContext;

        public RepositoryConnection()
        {
            _dbContext = new AmongUsContext();
        }

        public AmongUsContext Context { get  {return _dbContext; } }
    }
}