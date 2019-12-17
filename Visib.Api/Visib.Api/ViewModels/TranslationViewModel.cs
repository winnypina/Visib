using System;

namespace Visib.Api.ViewModels
{
    public class TranslationViewModel
    {
        public Guid Id { get; set; }

        public string Culture { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }
    }
}