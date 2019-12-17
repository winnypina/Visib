namespace Visib.Mobile.Services.Requests
{
    public class CodeVerificationRequest
    {
        public string UserName { get; set; }
        public int Code { get; set; }
    }
}