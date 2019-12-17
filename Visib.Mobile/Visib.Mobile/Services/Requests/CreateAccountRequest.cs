using System;

namespace Visib.Mobile.Services.Requests
{
    public class CreateAccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
        public string Picture { get; set; }
    }
}
