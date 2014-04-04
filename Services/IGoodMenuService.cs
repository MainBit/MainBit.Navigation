using MainBit.Navigation.Models;
using Orchard;

namespace MainBit.Navigation.Services
{
    public interface IGoodMenuService : IDependency
    {
        Menu GetMenu(int menuId);
        Menu GetCloneMenu(int menuId);
        void ResetCache(int menuId);
        string GetCacheKey(int menuId);
    }
}