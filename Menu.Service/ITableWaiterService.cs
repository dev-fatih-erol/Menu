﻿using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ITableWaiterService
    {
        void Delete(TableWaiter tableWaiter);

        List<TableWaiter> GetByTableId(int tableId);

        List<TableWaiter> GetByWaiterId(int waiterId);

        TableWaiter GetByTableIdAndWaiterId(int tableId, int waiterId);

        void Create(TableWaiter tableWaiter);

        void SaveChanges();
    }
}