using Menu.Core.Models;
using Menu.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Menu.Service
{
    public class DayReportsService : IDayReportsService
    {
        private readonly MenuContext _context;

        public DayReportsService(MenuContext context)
        {
            _context = context;
        }


        public void Create(DayReports dayReports)
        {
            _context.DayReportss.Add(dayReports);
        }

        public DayReports GetById(int id)
        {
            return _context.DayReportss
                             .Where(o => o.Id == id)
                             .FirstOrDefault();
        }

        public List<DayReports> GetByAllReport(int venueId)
        {
            return _context.DayReportss.Where(x => x.Venue.Id == venueId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}