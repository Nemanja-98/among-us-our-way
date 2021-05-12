
using System.Collections.Generic;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmongUs_OurWay.DataManagement
{
    public interface IRepository
    {
        IUserRepository GetUserRepository();

        IGameRepository GetGameRepository();
    }
}