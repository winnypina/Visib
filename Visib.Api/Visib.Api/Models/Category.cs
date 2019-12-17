using System;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string NameEnUs { get; set; }

        public string NameEs { get; set; }
    }
}
