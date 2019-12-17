using System;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.Models
{
    public class Translation
    {
        [Key] public Guid Id { get; set; }
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}