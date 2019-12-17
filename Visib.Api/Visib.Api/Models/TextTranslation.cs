using System;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.Models
{
    public class TextTranslation
    {
        [Required] public Guid Id { get; set; }

        public string Screen { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
        public string ValueEnUs { get; set; }
        public string ValueEs { get; set; }
    }
}