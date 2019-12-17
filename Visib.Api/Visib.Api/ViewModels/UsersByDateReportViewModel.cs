using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visib.Api.ViewModels
{
    public class UsersByDateReportViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
