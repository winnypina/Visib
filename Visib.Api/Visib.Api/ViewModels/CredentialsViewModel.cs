using FluentValidation.Attributes;
using Visib.Api.ViewModels.Validations;

namespace Visib.Api.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}