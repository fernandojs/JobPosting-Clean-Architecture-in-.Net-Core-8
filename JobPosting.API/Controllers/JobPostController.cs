using Microsoft.AspNetCore.Mvc;
using JobPostingDTO.Application.Interfaces;
using JobPosting.Application.DTOs;
using Microsoft.AspNetCore.Authorization; 

namespace JobPosting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobPostController : ControllerBase
    {
        private readonly IJobPostService _jobPostService;

        public JobPostController(IJobPostService jobPostService)
        {
            _jobPostService = jobPostService;
        }

        // GET: api/JobPost
        [HttpGet]
        public async Task<IActionResult> GetJobPosts()
        {
            var jobPosts = await _jobPostService.GetJobPostsAsync();
            return Ok(jobPosts);
        }

        // GET: api/JobPost/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobPost(int id)
        {
            var jobPost = await _jobPostService.GetByIdAsync(id);
            if (jobPost == null)
            {
                return NotFound();
            }
            return Ok(jobPost);
        }

        // POST: api/JobPost
        [HttpPost]
        public async Task<IActionResult> CreateJobPost([FromBody] JobPostDTO jobPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newJobPostId = await _jobPostService.CreateAsync(jobPostDTO);
            jobPostDTO.Id = newJobPostId; 
            return CreatedAtAction(nameof(GetJobPost), new { id = newJobPostId }, jobPostDTO);
        }

        // PUT: api/JobPost/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobPost(int id, [FromBody] JobPostDTO jobPostDTO)
        {
            if (id != jobPostDTO.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _jobPostService.UpdateAsync(jobPostDTO);
            return NoContent();
        }

        // DELETE: api/JobPost/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPost(int id)
        {
            var jobPostDTO = await _jobPostService.GetByIdAsync(id);
            if (jobPostDTO == null)
            {
                return NotFound();
            }

            await _jobPostService.RemoveAsync(jobPostDTO);
            return NoContent();
        }
    }
}
