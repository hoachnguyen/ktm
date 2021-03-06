﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using KMS.Product.Ktm.Api.Exceptions;
using KMS.Product.Ktm.Entities.DTO;
using KMS.Product.Ktm.Services.KudoService;

namespace KMS.Product.Ktm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {

        private readonly IKudoService _kudoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject Kudo service
        /// </summary>
        /// <returns></returns>
        public ReportController(IKudoService kudoService, IMapper mapper)
        {
            _kudoService = kudoService ?? throw new ArgumentNullException($"{nameof(kudoService)}");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)}");
        }

        /// <summary>
        /// Get kudos dto for report
        /// </summary>
        /// <returns>
        /// Success: returns 200 status code with a collection of all kudos        
        /// Failure: returns 500 status code with an exception message
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetKudosForReport(
            [FromQuery] DateTime? dateFrom, 
            [FromQuery] DateTime? dateTo,
            [FromQuery] List<int> teamIds,
            [FromQuery] List<int> kudoTypeIds)
        {
            try
            {
                IEnumerable<KudoDetailDto> kudos = await _kudoService.GetKudosForReport(dateFrom, dateTo, teamIds, kudoTypeIds);
                return Ok(kudos);
            }
            catch (BussinessException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get kudos summary for report
        /// </summary>
        /// <returns>
        /// Success: returns 200 status code with a collection of all kudos        
        /// Failure: returns 500 status code with an exception message
        /// </returns>
        [HttpGet("summary")]
        public async Task<IActionResult> GetKudosummaryForReport(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] List<int> filterIds,
            [FromQuery] int type)
        {
            try
            {
                IEnumerable<KudoSumReportDto> kudos = await _kudoService.GetKudosummaryForReport(dateFrom, dateTo, filterIds, type);
                return Ok(kudos);
            }
            catch (BussinessException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
