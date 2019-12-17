using System;
using System.ComponentModel.DataAnnotations;

namespace Visib.Api.Models
{
    public class PrivacyPolicy
    {
        [Key] public Guid Id { get; set; }

        public string Value { get; set; }

        public string ValueEnUs { get; set; }

        public string ValueEs { get; set; }
    }
}