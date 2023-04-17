﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.Interfaces
{
    public interface ISaleRepository:IGenericRepository<Sale>
    {
        Task<Sale> Registry(Sale entity);
        Task<List<Sale>> Report(DateTime StartDate, DateTime EndDate);
    }
}
