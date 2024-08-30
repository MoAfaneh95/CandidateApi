using CandidateApi.Models;
using CandidateApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly IConfiguration _conf;

        public CandidatesController(IConfiguration configuration,ICandidateService candidateService)
        {
            _candidateService = candidateService;
            _conf = configuration;
        }

        [HttpPost("AddOrUpdateCandidate")]
        public IActionResult AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            var updatedCandidate = _candidateService.AddOrUpdateCandidate(candidate);
            return Ok(updatedCandidate);
        }
        [HttpPost("AddOrUpdateCandidates")]
        public IActionResult AddOrUpdateCandidates([FromBody] IList<Candidate> candidates)
        {
            if (candidates.Any())
            {
                candidates = _candidateService.AddOrUpdateCandidates(candidates);
                return Ok(candidates);
            }
            else
                return BadRequest("ERROR : No Data Passing");
        }
    }
}
