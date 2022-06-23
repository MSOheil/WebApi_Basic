using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;

namespace WebApiBasic.Repositories
{
    public interface IVisitService
    {
        Task<Visit> GetVisitsWithIdAsync(Guid idVisit);
        Task<bool> DeleteVisitsAsync(Visit visit);
        Task<bool> UpdateVisitsAsync(Visit visit);
        Task<bool> InsertVistAsync(Visit visit);
        void Save();
       IEnumerable<VisitReadDto> PagingAndSortAndFilterVisits(int pageNumber = 1, int pageResult = 5, bool sortByBirthday = false, DateTime? filterByCreateAtd = null);
    }
}
