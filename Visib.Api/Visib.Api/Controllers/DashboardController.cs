using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visib.Api.Data;
using Visib.Api.Models;
using Visib.Api.ViewModels;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public DashboardController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("UsersByDate")]
        public async Task<ActionResult<List<UsersByDateReportViewModel>>> GetUsersByDate(Guid id)
        {
            var refDate = DateTime.Now.Date.AddDays(-30);
            var data =  await _appDbContext.UsersByDate.Where(n => n.Date > refDate).ToListAsync();
            var list = new List<UsersByDateReportViewModel>();
            for (var x = 0; x < 30; x++)
            {
                var currentDate = DateTime.Now.Date.AddDays(-x);
                var dateInList = data.SingleOrDefault(n => n.Date == currentDate);
                UsersByDateReportViewModel dateReport;
                if (dateInList == null)
                {
                    dateReport = new UsersByDateReportViewModel
                    {
                        Date = currentDate,
                        Id = Guid.NewGuid(),
                        NumberOfUsers = 0
                    };
                }
                else
                {
                    dateReport = new UsersByDateReportViewModel
                    {
                        Date = currentDate,
                        Id = dateInList.Id,
                        NumberOfUsers = dateInList.NumberOfUsers 
                    };
                }
                list.Add(dateReport);
            }

            return new OkObjectResult(list.OrderBy(n=>n.Date));
        }

    }
}