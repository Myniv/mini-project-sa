﻿using Asp.Versioning;
using CompanyWeb.Application.Repositories;
using CompanyWeb.Application.Services;
using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompanyWeb.WebApi.Controllers
{
    public class DepartementsController : BaseController
    {
        private readonly IDepartementService _departementService;
        public DepartementsController(IDepartementService departementService)
        {
            _departementService = departementService;
        }
        /// <summary>
        /// Get all departements with paging
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Departements?pageNumber=1ANDperPage=5
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return departement list with paging </returns>
        // GET: api/Departements
        [HttpGet]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Departement>>> GetDepartements([FromQuery] int pageNumber, int perPage)
        {
            return await _departementService.GetDepartements(pageNumber, perPage);
        }

        /// <summary>
        /// Get departement by ID
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Departements/1
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return departement data by ID </returns>
        // GET: api/Departements/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Departement>> GetDepartement([FromRoute]int id)
        {
            var departement = await _departementService.GetDepartement(id);
            if (departement == null)
            {
                return NotFound();
            }
            return departement;
        }


        /// <summary>
        /// Create departement
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Departements
        ///     {
        ///         "deptName": "Finance",
        ///         "mgrEmpNo": null
        ///     }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return departement data that just created </returns>
        // POST: api/Departements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Departement>> PostDepartement([FromBody] AddDepartementRequest request)
        {
            var action = await _departementService.CreateDepartement(request);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }


        /// <summary>
        /// Update departement by ID
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Departements/1
        ///     {
        ///         "deptName": "Finance",
        ///         "mgrEmpNo": 2
        ///     }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return departement data that just updated </returns>
        // PUT: api/Departements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDepartement([FromRoute]int id, [FromBody] UpdateDepartementRequest request)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var action = await _departementService.UpdateDepartement(id, request);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }

        /// <summary>
        /// Delete departement by ID
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Departements/1
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return departement data that just deleted </returns>
        // DELETE: api/Departements/5
        [ProducesResponseType(typeof(Departement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Departement), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartement([FromRoute]int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var action = await _departementService.DeleteDepartement(id);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }
    }
}