using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Visib.Api.Models
{
    public class UsersByDateReport
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
