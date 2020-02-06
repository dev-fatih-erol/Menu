using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IAppAboutService
    {
        List<AppAbout> Get(bool status);

        void Create(AppAbout appAbout);

        void SaveChanges();
    }
}