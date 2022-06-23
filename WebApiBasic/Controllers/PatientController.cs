using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;
using WebApiBasic.Repositories;

namespace WebApiBasic.Controllers
{
    [ApiController]
    [Route("api/patient")]
    [Authorize(Roles = "Admin")]
    public class PatientController : ControllerBase
    {
        private IMapper _mapper;
        private IPatientService _patientRepository;
        private readonly ILogger<PatientController> _logger;
        public PatientController(IPatientService patientRepository, IMapper mapper,ILogger<PatientController> logger)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public  ActionResult<IEnumerable<ReadPatientDto>> GetAllPatientAsync(int pageNumber = 1, int pageResult = 5, bool sortByBirthday = false, string namePatientForFilter = null)
        {
            var allPatient =  _patientRepository.PagingAndSortAndFilter(pageNumber, pageResult, sortByBirthday, namePatientForFilter);
            return Ok(allPatient);
        }



        [HttpGet("{patientId}", Name = "GetPatientAsync")]
        public async Task<ActionResult<ReadPatientWithIdDto>> GetPatientWithIdAsync(Guid patientId)
        {
            var patinetById = await _patientRepository.GetPatientWithId(patientId);
            var readDto = _mapper.Map<ReadPatientWithIdDto>(patinetById);
            if (patinetById == null)
            {
                return NotFound();
            }

            return Ok(readDto);
        }
        [HttpPost]
        public async Task<ActionResult> CreatePatientAsync([FromBody] CreatePatientDto createPatientDto)
        {

            var inputPatient = _mapper.Map<Patient>(createPatientDto);

            inputPatient.IsActive = true;

            var createPatient = await _patientRepository.InsertPatient(inputPatient);

            _patientRepository.Save();

            var patientReadDto = _mapper.Map<ReadPatientDto>(inputPatient);

            if(createPatient ==true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("{patientId}")]
        public async Task<ActionResult> UpdatePutVisitAsync(Guid patientId, [FromBody] CreatePatientDto updatePatientDto)
        {
            var findePatient = await _patientRepository.GetPatientWithId(patientId);
            if (findePatient == null)
            {
                return NotFound();
            }
            _mapper.Map(updatePatientDto, findePatient);
            await _patientRepository.UpdatePatient(findePatient);
            _patientRepository.Save();
            return Ok();
        }
        [HttpDelete("{patientId}")]
        public async Task<ActionResult> DeletePatient(Guid patientId)
        {
            var findePatient = await _patientRepository.GetPatientWithId(patientId);
            findePatient.IsActive = false;
            if (findePatient == null)
            {
                return NotFound();
            }
            await _patientRepository.DeletePatient(findePatient);
            _patientRepository.Save();
            return Ok();
        }
    }
}
