using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Visib.Api.Models
{
    public class UseTerm 
    {
        [Key]
        public Guid Id { get; set; }

        public string Value { get; set; }

        public string ValueEnUs { get; set; }

        public string ValueEs { get; set; }
    }
}
