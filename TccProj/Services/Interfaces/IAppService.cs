using Firebase.Database;

namespace TccProj.Services.Interfaces
{
    public interface IAppService
    {
        FirebaseClient Client();
    }
}
