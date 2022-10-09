﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalkDifficultiesController : Controller
    {

        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;
        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            //fetch data from database- domain walks
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();
            //convert domain walks to DTO walks
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);

            // return Response

            return Ok(walkDifficultyDTO);
           
            //Or a single line is enough

            ///return Ok(await walkDifficultyRepository.GetAllSync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultiesAsync")]
        public async Task<IActionResult> GetWalkDifficultiesAsync(Guid id)
        {
            // Get WalkDifficulty Domain object from database
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficultyDomain == null)
            {
                return NotFound();

            }
            //convert domain object to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //Return response
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert DTO to Domain Object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
                
            };

            //Pass Domain object to Repository to persist this
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert Domain Object back to DTO

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
               Code= walkDifficultyDomain.Code

            };

            //Send DTO responseback to client

            return CreatedAtAction(nameof(GetWalkDifficultiesAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
       

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //Call Repository to delete
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);


            // If Null Not Found

            if (walkDifficultyDomain == null)
            {
                return NotFound();

            }
            // Convert response back To DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);




            //return OK response
            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)

        {
            // Convert DTO to domain model

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
                
            };
            // Update Region using Repository

            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            // If Null then Not Found

            if (walkDifficultyDomain == null)
            {
                return NotFound();

            }

            // Convert Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()

            {
                Code = walkDifficultyDomain.Code
                
               

            };


            // Return Ok response

            return Ok(walkDifficultyDTO);
        }
    }
}