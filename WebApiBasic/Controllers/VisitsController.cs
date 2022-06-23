using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;
using WebApiBasic.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/visits")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitRepository;
        private readonly IMapper _mapper;
        public VisitsController(IVisitService visitRepository, IMapper mapper)
        {
            _visitRepository = visitRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<VisitReadDto>> GetAllVisitsAsync(int pageNumber = 1, int pageResult = 5, bool sortByBirthday = false, DateTime? filterByCreateAtd = null)
        {
            var allVisits =  _visitRepository.PagingAndSortAndFilterVisits(pageNumber,pageResult,sortByBirthday, filterByCreateAtd);
            return Ok(allVisits);
        }



        [HttpGet("{idVisit}", Name = "GetAllVisitsAsync")]
        public async Task<ActionResult<Visit>> GetAllVisitsWithIdAsync(Guid idVisit)
        {
            var visitWithId = await _visitRepository.GetVisitsWithIdAsync(idVisit);
            if (visitWithId == null)
            {
                return NotFound();
            }
            return Ok(visitWithId);
        }
        [HttpPost]
        public async Task<ActionResult> CreateVisitAsync([FromBody] InsertVisitDto createVisitDto)
        {

            var mapDtoToModel = _mapper.Map<Visit>(createVisitDto);

            mapDtoToModel.IsActive = true;

            var inserVisit = await _visitRepository.InsertVistAsync(mapDtoToModel);

            _visitRepository.Save();

            var visitreadDto = _mapper.Map<VisitReadDto>(mapDtoToModel);
            if (inserVisit == true)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("/api/visits/complete/{visitId}")]
        public async Task<ActionResult> UpdatePutAsync(Guid visitId, [FromBody] VisitUpdateDto updateVisitDto)
        {
            var findevisit = await _visitRepository.GetVisitsWithIdAsync(visitId);
            if (findevisit == null)
            {
                return NotFound();
            }
            _mapper.Map(updateVisitDto, findevisit );
            await _visitRepository.UpdateVisitsAsync(findevisit);
            _visitRepository.Save();

            return Ok();
        }
        [HttpDelete("{visitId}")]
        public async Task<ActionResult> DeleteVisitAsync(Guid visitId)
        {
            var findevisit = await _visitRepository.GetVisitsWithIdAsync(visitId);
            findevisit.IsActive = false;
            if (findevisit == null)
            {
                return NotFound();
            }
            await _visitRepository.DeleteVisitsAsync(findevisit);
            _visitRepository.Save();

            return Ok();
        }
    }
}
