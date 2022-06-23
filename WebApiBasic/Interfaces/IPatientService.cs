using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;

namespace WebApiBasic.Repositories
{
    public interface IPatientService
    {
        Task<Patient> GetPatientWithId(Guid idPatient);
        Task<bool> DeletePatient(Patient patient);
        Task<bool> UpdatePatient(Patient patient);
        Task<bool> InsertPatient(Patient patient);
        int VisitCount();
        void Save();
        IEnumerable<ReadPatientDto> PagingAndSortAndFilter(int pageNumber = 1, int pageResult = 5, bool sortByBirthday = false, string namePatientForFilter = "");
    }
}
