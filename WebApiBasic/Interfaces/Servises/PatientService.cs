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
    public class PatientService : IPatientService
    {
        private DBContext _db;
        public PatientService(DBContext db)
        {
            _db = db;
        }
        public async Task<bool> DeletePatient(Models.Patient patient)
        {
            try
            {
                var patientById = await GetPatientWithId(patient.PatientId);
                patientById = patient;
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Models.Patient> GetPatientWithId(Guid idPatient)
        {
            var patientById = _db.Patients.Include(a=>a.Visits).SingleOrDefault(a => a.PatientId == idPatient);
            return await Task.FromResult(patientById);
        }

        public async Task<bool> InsertPatient(Models.Patient patient)
        {
            try
            {
                _db.Patients.Add(patient);
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        private IEnumerable<Patient> FilterAndSortingAdnPagingPatient(int pageNumber, int pageResult, bool sortByBirthday, string namePatientForFilter)
        {
            var patients =  _db.Patients.AsNoTracking().ToList();
            if (sortByBirthday == true)
            {
                patients = patients.OrderBy(a => a.Birthday).ToList();
            }
            if (namePatientForFilter != null)
            {
                patients = patients.Where(a => a.Name == namePatientForFilter).ToList();
            }
            return patients =  patients.Where(a=>a.IsActive==true).Skip(pageResult * (pageNumber - 1)).Take(pageResult).ToList();
        }
        public IEnumerable<ReadPatientDto> PagingAndSortAndFilter(int pageNumber = 1, int pageResult = 5, bool sortByBirthday = false, string namePatientForFilter = "")
        {
            var patient = FilterAndSortingAdnPagingPatient(pageNumber, pageResult, sortByBirthday, namePatientForFilter);
            var dtoRead = patient.Select(a => new ReadPatientDto()
            {
                PatientId = a.PatientId,
                Name = a.Name,
                NationalCode = a.NationalCode,
                Birthday = a.Birthday,
                visitCount= VisitCount()
            }).ToList();
            return dtoRead;
        }
        public async Task<bool> UpdatePatient(Models.Patient patient)
        {
            try
            {
                var patientById = await GetPatientWithId(patient.PatientId);
                patientById = patient;
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int VisitCount()
        {
            return _db.Visits.Count();
            
        }
    }
}
