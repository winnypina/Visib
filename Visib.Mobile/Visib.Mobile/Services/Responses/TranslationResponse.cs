using System;

namespace Visib.Mobile.Services.Responses
{
    public class TranslationResponse
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Screen { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
        public string ValueEnUs { get; set; }
        public string ValueEs { get; set; }
    }
}