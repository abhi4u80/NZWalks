using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch data from database- domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            //convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // return Response

            return Ok(walksDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);

            if (walkDomain == null)
            {
                return NotFound();

            }
            //convert domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //Return response
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionID= addWalkRequest.RegionID,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
                
            };

            //Pass Domain object to Repository to persist this
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert Domain Object back to DTO

            var walkDTO = new Models.DTO.Walk
            {
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };

            //Send DTO responseback to client

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)

        {
            // Convert DTO to domain model

            var walkDomain = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionID = updateWalkRequest.RegionID,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            // Update Region using Repository

           walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            // If Null then Not Found

            if (walkDomain == null)
            {
                return NotFound();

            }

            // Convert Domain back to DTO
            var walkDTO = new Models.DTO.Walk()

            {
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };


            // Return Ok response

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Call Repository to delete
            var walkDomain = await walkRepository.DeleteAsync(id);


            // If Null Not Found

            if (walkDomain == null)
            {
                return NotFound();

            }
            // Convert response back To DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

           


            //return OK response
            return Ok(walkDTO);
        }
    }
}
