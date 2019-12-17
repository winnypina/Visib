using System;

namespace Visib.Api.ViewModels
{
    public class UpdateRegistrationViewModel
    {
        public string CpfCnpj { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
    }
}