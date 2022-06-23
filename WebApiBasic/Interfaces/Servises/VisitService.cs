using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Data;
using WebApiBasic.Dtos;
using WebApiBasic.Models;
using WebApiBasic.Repositories;

namespace WebApiBasic.Servises
{
    public class VisitService : IVisitService
    {
        private DBContext _db;

        public VisitService(DBContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteVisitsAsync(Visit visit)
        {
            try
            {
                var visitById = await GetVisitsWithIdAsync(visit.VisitId);
                visitById = visit;
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Visit> GetVisitsWithIdAsync(Guid idVisit)
        {
            var visitWithId = _db.Visits.Where(a => a.VisitId == idVisit).SingleOrDefault();
            return await Task.FromResult(visitWithId);
        }

        public async Task<bool> InsertVistAsync(Visit visit)
        {
            try
            {
                _db.Visits.Add(visit);
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }
        private IQueryable<Visit> FilterAndSortingAdnPagingVisits(int pageNumber, int pageResult, bool sortByVisitedAt, DateTime? filterByCreateAtd)
        {
            var Visits = _db.Visits.AsNoTracking().AsQueryable();
            if (sortByVisitedAt == true)
            {
                Visits = Visits.OrderBy(a => a.VisitedAt);
            }
            if (filterByCreateAtd != null)
            {
                Visits = Visits.Where(a => a.CreateAt == filterByCreateAtd);
            }
            return Visits = Visits.Where(a => a.IsActive == true).Skip(pageResult * (pageNumber - 1)).Take(pageResult);
        }
        public IEnumerable<VisitReadDto> PagingAndSortAndFilterVisits(int pageNumber = 1, int pageResult = 5, bool sortByVisitedAt = false, DateTime? filterByCreateAt = null)
        {
            var visitis = FilterAndSortingAdnPagingVisits(pageNumber, pageResult, sortByVisitedAt, filterByCreateAt);
            var dtoReadVisits = visitis.Select(a => new VisitReadDto()
            {
                VisitId = a.VisitId,
                CreateAt = a.CreateAt,
                PatientId = a.PatientId,
                VisitedAt = a.VisitedAt
            });
            return dtoReadVisits.ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<bool> UpdateVisitsAsync(Visit visit)
        {
            try
            {
                var visitById = await GetVisitsWithIdAsync(visit.VisitId);
                visitById = visit;

                await Task.CompletedTask;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
