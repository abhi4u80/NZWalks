using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalkDbContext nZWalksDbContext;

        public WalkRepository(NZWalkDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Asign New Id
            walk.Id = Guid.NewGuid();
           await nZWalksDbContext.Walks.AddAsync(walk); 
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingwalk = await nZWalksDbContext.Walks.FindAsync(id);

            if (existingwalk == null)
            {
                return null;

            }
            // Delete walk

            nZWalksDbContext.Walks.Remove(existingwalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;

            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionID = walk.RegionID;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;


            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
