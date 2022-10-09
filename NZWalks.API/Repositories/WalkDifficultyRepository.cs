using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalkDbContext nZWalksDbContext;
        public  WalkDifficultyRepository(NZWalkDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext= nZWalksDbContext;
        }
       
        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            //Asign New Id
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;
        }
        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingwalkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);

            if (existingwalkDifficulty == null)
            {
                return null;

            }
            // Delete walk

            nZWalksDbContext.WalkDifficulty.Remove(existingwalkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return existingwalkDifficulty;
        }
        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalkDifficulty == null)
            {
                return null;

            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
           

            await nZWalksDbContext.SaveChangesAsync();

            return existingWalkDifficulty;
        }
    }
}
