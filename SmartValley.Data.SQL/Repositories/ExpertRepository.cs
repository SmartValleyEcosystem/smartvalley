using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExpertRepository : IExpertRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ExpertRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public Task<int> RemoveAsync(Expert expert)
        {
            _editContext.Experts.Attach(expert);
            _editContext.Experts.Remove(expert);
            return _editContext.SaveAsync();
        }

        public Task<Expert> GetByAddressAsync(Address address)
        {
            return (from expert in _readContext.Experts
                    join user in _readContext.Users on expert.UserId equals user.Id
                    where user.Address == address
                    select expert).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Area>> GetAreasAsync()
            => await _readContext.Areas.ToArrayAsync();

        public Task AddAsync(long expertId, IReadOnlyCollection<int> areas)
        {
            var expert = new Expert {UserId = expertId, IsAvailable = true};
            _editContext.Experts.Add(expert);

            var expertAreas = areas.Select(s => new ExpertArea
                                                {
                                                    Expert = expert,
                                                    AreaId = (AreaType) s
                                                })
                                   .ToArray();

            _editContext.ExpertAreas.AddRange(expertAreas);
            return _editContext.SaveAsync();
        }

        public async Task UpdateAsync(Expert expert, IReadOnlyCollection<int> areas)
        {
            _editContext.Experts.Attach(expert).State = EntityState.Modified;

            var expertAreas = await _editContext.ExpertAreas.Where(e => e.ExpertId == expert.UserId).ToArrayAsync();
            _editContext.ExpertAreas.RemoveRange(expertAreas);
            _editContext.ExpertAreas.AddRange(areas.Select(s => new ExpertArea
                                                                {
                                                                    Expert = expert,
                                                                    AreaId = (AreaType) s
                                                                }).ToArray());
            await _editContext.SaveAsync();
        }

        public Task<Expert> GetAsync(long expertId)
            => _readContext.Experts.FirstOrDefaultAsync(e => e.UserId == expertId);

        public async Task SetAvailabilityAsync(long expertId, bool isAvailable)
        {
            var expert = await _editContext.Experts.FirstOrDefaultAsync(e => e.UserId == expertId);
            if (expert == null)
                throw new AppErrorException(ErrorCode.ExpertNotFound);

            expert.IsAvailable = isAvailable;
            await _editContext.SaveAsync();
        }

        public Task<int> GetTotalCountExpertsAsync()
            => _readContext.Experts.CountAsync();

        public async Task<ExpertDetails> GetDetailsAsync(Address address)
        {
            var expertUser = await (from expert in _readContext.Experts
                                    join user in _readContext.Users on expert.UserId equals user.Id
                                    where user.Address == address
                                    select new {expert, user})
                                 .FirstOrDefaultAsync();

            var expertAreas = await (from expertArea in _readContext.ExpertAreas
                                     join area in _readContext.Areas on expertArea.AreaId equals area.Id
                                     where expertArea.ExpertId == expertUser.user.Id
                                     select area)
                                  .ToArrayAsync();

            return new ExpertDetails
            (
                expertUser.user.Address,
                expertUser.user.Email,
                expertUser.user.Name,
                expertUser.user.About,
                expertUser.expert.IsAvailable,
                expertAreas
            );
        }

        public async Task<IReadOnlyCollection<ExpertDetails>> GetAllDetailsAsync(int offset, int count)
        {
            var expertUsersQuery = (from expert in _readContext.Experts
                                    join user in _readContext.Users on expert.UserId equals user.Id
                                    select new {expert, user})
                                   .Skip(offset)
                                   .Take(count);

            var expertAreas = await (from expertUser in expertUsersQuery
                                     join expertArea in _readContext.ExpertAreas on expertUser.user.Id equals expertArea.ExpertId
                                     join area in _readContext.Areas on expertArea.AreaId equals area.Id
                                     select new {expertArea.ExpertId, area}).ToArrayAsync();

            var lookUpAreas = expertAreas.ToLookup(k => k.ExpertId, v => v.area);

            return await expertUsersQuery.Select(expertUser =>
                                                     new ExpertDetails(expertUser.user.Address,
                                                                       expertUser.user.Email,
                                                                       expertUser.user.Name,
                                                                       expertUser.user.About,
                                                                       expertUser.expert.IsAvailable,
                                                                       lookUpAreas[expertUser.expert.UserId].ToArray())).ToArrayAsync();
        }
    }
}