using EngMonarchApi.Data;
using EngMonarchApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EngMonarchApi.Controllers
{
    /// <summary>
    /// EngMonarchs endpoint of EngMonarchs API.
    /// </summary>
    [ApiController]
    [ApiVersion("2")]
    //[Authorize]
    [Route("api/v{api-version:apiVersion}/engmonarchs")]
    public class EngMonarchV2Controller : Controller
    {
        private readonly IEngMonarchRepository _engmonarchsRepository;
        private readonly ILogger<EngMonarchV2Controller> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="EngMonarchV2Controller"/> with dependencies injected.
        /// </summary>
        /// <param name="engmonarchsRepository">A repository for managing the engmonarchs.</param>
        /// <param name="logger">Logger implementation.</param>
        public EngMonarchV2Controller(IEngMonarchRepository engmonarchsRepository, ILogger<EngMonarchV2Controller> logger)
        {
            _engmonarchsRepository = engmonarchsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Delete the engmonarch with the given id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to delete.</param>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult Delete(string id)
        {
            _logger.LogInformation($"Deleting engmonarch with id {id}");

            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var engmonarch = _engmonarchsRepository.GetByIdAsync(id);
            if (engmonarch == null)
                return NotFound();

            _engmonarchsRepository.DeleteAsync(id);

            return Ok();
        }

        /// <summary>
        /// Get one page of engmonarchs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Page size.</param>
        /// <remarks>If you omit <c>page</c> and <c>size</c> query parameters, you'll get the first page with 10 engmonarchs.</remarks>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PagedList<EngMonarch>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PagedList<EngMonarch>>> GetAll(int page = 1, int size = 10)
        {
            _logger.LogInformation("Getting one page of engmonarchs");

            var engmonarchs = await _engmonarchsRepository.GetPageAsync(page, size);

            return engmonarchs;
        }

        /// <summary>
        /// Get a single engmonarch by id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(EngMonarch))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EngMonarch>> GetById(string id)
        {
            _logger.LogInformation($"Getting a engmonarch with id {id}");

            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var engmonarch = await _engmonarchsRepository.GetByIdAsync(id);

            if (engmonarch == null)
                return NotFound();

            return engmonarch;
        }

        /// <summary>
        /// Create a new engmonarch from the supplied data.
        /// </summary>
        /// <param name="model">Data to create the engmonarch from.</param>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EngMonarch))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public ActionResult<EngMonarch> Post(EngMonarchInput model)
        {
            _logger.LogInformation($"Creating a new engmonarch with title \"{model.Name}\"");

            var engmonarch = new EngMonarch();
            model.MapToEngMonarch(engmonarch);

            _engmonarchsRepository.CreateAsync(engmonarch);

            return CreatedAtAction(nameof(GetById), "engmonarchs", new { id = engmonarch.Id }, engmonarch);
        }

        /// <summary>
        /// Updates the engmonarch with the given id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to update.</param>
        /// <param name="model">Data to update the engmonarch from.</param>
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(EngMonarch))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EngMonarch>> Put(string id, EngMonarchInput model)
        {
            _logger.LogInformation($"Updating a engmonarch with id {id}");

            var engmonarch = await _engmonarchsRepository.GetByIdAsync(id);

            if (engmonarch == null)
                return NotFound();

            model.MapToEngMonarch(engmonarch);

            _engmonarchsRepository.UpdateAsync(engmonarch);

            return engmonarch;
        }
    }
}
