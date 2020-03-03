using Menu.Core.Models;
using System.Collections.Generic;

namespace Menu.Service
{
    public interface IDayReportsService
    {
        DayReports GetById(int id);

        List<DayReports> GetByAllReport(int venueId);

        void Create(DayReports dayReports);

        void SaveChanges();
    }
}