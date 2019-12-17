using MonkeyCache.SQLite;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Visib.Mobile.ViewModels;

namespace Visib.Mobile
{
    public class VisibApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            RegisterAppStart<SplashViewModel>();
            Barrel.ApplicationId = "Visib";
        }
    }
}
