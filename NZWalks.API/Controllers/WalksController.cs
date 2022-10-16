using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionrepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionrepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Validate the incoming request

            if (!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);

            }

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
        [Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)

        {
            //Validate the incoming request

            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);

            }

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
        [Authorize(Roles = "writer")]
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

        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                    $"Data is required");

            }


            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} cannot be empty or null or whitespace");

            //}

            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} cannot be less than or equal to zero");

            //}


            var region = await regionrepository.GetAsync(addWalkRequest.RegionID);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionID),
                    $"{nameof(addWalkRequest.RegionID)} is invalid");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                    $"Data is required");

            }


            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} cannot be empty or null or whitespace");

            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} cannot be less than or equal to zero");

            //}


            var region = await regionrepository.GetAsync(updateWalkRequest.RegionID);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionID),
                    $"{nameof(updateWalkRequest.RegionID)} is invalid");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
    }

    #endregion
}



