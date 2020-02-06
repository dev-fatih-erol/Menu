using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IAppSliderService
    {
        List<AppSlider> Get(bool status);

        void Create(AppSlider appSlider);

        void SaveChanges();
    }
}