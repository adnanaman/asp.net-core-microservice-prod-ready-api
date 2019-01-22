using EngMonarchApi.Data;
using EngMonarchApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace EngMonarchApi.Controllers
{
    /// <summary>
    /// EngMonarchs endpoint of EngMonarchs API.
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiVersion("1", Deprecated = true)]
    //[Authorize]
    [Route("api/engmonarchs")]
    [Route("api/v{api-version:apiVersion}/engmonarchs")]
    public class EngMonarchController : Controller
    {
        private readonly IEngMonarchRepository _engmonarchRepository;
        private readonly ILogger<EngMonarchController> _logger;
        private readonly string _connectionString;
        private readonly ETagRedisCache _cache;

        /// <summary>
        /// Creates a new instance of <see cref="EngMonarchController"/> with dependencies injected.
        /// </summary>
        /// <param name="engmonarchRepository">A repository for managing the engmonarch.</param>
        /// <param name="logger">Logger implementation.</param>
        public EngMonarchController(IEngMonarchRepository engmonarchRepository, 
            ILogger<EngMonarchController> logger,
            IConfiguration configuration,
            ETagRedisCache cache)
        {
            _engmonarchRepository = engmonarchRepository;
            _logger = logger;

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _cache = cache;
        }

        /// <summary>
        /// Delete the engmonarch with the given id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to delete.</param>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _logger.LogInformation($"Deleting engmonarch with id {id}");

            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var engmonarch = _engmonarchRepository.GetByIdAsync(id);
            if (engmonarch == null)
                return NotFound();

            _engmonarchRepository.DeleteAsync(id);

            return Ok();
        }

        /// <summary>
        /// Get one page of engmonarch.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Page size.</param>
        /// <remarks>If you omit <c>page</c> and <c>size</c> query parameters, you'll get the first page with 10 engmonarch.</remarks>

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PagedList<EngMonarch>))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedList<EngMonarch>>> GetAll(int page = 1, int size = 10)
        {
            bool isModified = false;
            _logger.LogInformation("Getting one page of engmonarch");
            var engmonarch = _cache.GetCachedObject<PagedList<EngMonarch>>($"GetAll-{page}-{size}");
            if (engmonarch == null)
            {
                engmonarch = await _engmonarchRepository.GetPageAsync(page, size);
                isModified = _cache.SetCachedObject($"GetAll-{page}-{size}", engmonarch);
            }

            //isModified = _cache.SetCachedObject($"GetAll-{page}-{size}", engmonarch);

            if (isModified)
            {
                return Ok(engmonarch);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }
        }

        /// <summary>
        /// Get a single engmonarch by id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EngMonarch))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EngMonarch>> GetById(string id)
        {
            _logger.LogInformation($"Getting a engmonarch with id {id}");

            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var engmonarch = await _engmonarchRepository.GetByIdAsync(id);

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EngMonarch))]
        public ActionResult<EngMonarch> Post(EngMonarchInput model)
        {
            _logger.LogInformation($"Creating a new engmonarch with title \"{model.Name}\"");

            var engmonarch = new EngMonarch();
            model.MapToEngMonarch(engmonarch);

            _engmonarchRepository.CreateAsync(engmonarch);

            return CreatedAtAction(nameof(GetById), "engmonarch", new {id = engmonarch.Id}, engmonarch);
        }

        /// <summary>
        /// Updates the engmonarch with the given id.
        /// </summary>
        /// <param name="id">Id of the engmonarch to update.</param>
        /// <param name="model">Data to update the engmonarch from.</param>
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EngMonarch))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EngMonarch>> Put(string id, EngMonarchInput model)
        {
            _logger.LogInformation($"Updating a engmonarch with id {id}");

            var engmonarch = await _engmonarchRepository.GetByIdAsync(id);

            if (engmonarch == null)
                return NotFound();

            model.MapToEngMonarch(engmonarch);

            _engmonarchRepository.UpdateAsync(engmonarch);

            return engmonarch;
        }
    }
}
