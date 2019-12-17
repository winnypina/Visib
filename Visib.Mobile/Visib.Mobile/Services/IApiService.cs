namespace Visib.Mobile.Services
{
    public interface IApiService
    {
        IVisibService Speculative { get; }
        IVisibService UserInitiated { get; }
        IVisibService Background { get; }
    }
}
