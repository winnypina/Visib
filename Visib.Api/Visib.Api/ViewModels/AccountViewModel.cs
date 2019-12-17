using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visib.Api.Models;

namespace Visib.Api.ViewModels
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }
        public int Code { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobilePhone { get; set; }
        public long? FacebookId { get; set; }
        public bool HasSetPassword { get; set; }
        public string DesiredLanguage { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string AddressNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string AddressAddon { get; set; }
        public string Country { get; set; }
        public int Followers { get; set; }
        public int Follows { get; set; }
    }
}
