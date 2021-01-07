using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AmongUs_OurWay.Models;
using Microsoft.EntityFrameworkCore;

namespace AmongUs_OurWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private AmongUsContext dbContext;

        public WeatherForecastController(AmongUsContext db)
        {
            dbContext = db;
        }
    }
}