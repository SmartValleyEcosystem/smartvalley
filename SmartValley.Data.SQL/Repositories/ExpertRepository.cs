using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
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

        public Task<Expert> GetByAddressAsync(string address)
        {
            return (from expert in _readContext.Experts
                    join user in _readContext.Users on expert.UserId equals user.Id
                    where user.Address.Equals(address, StringComparison.OrdinalIgnoreCase)
                    select expert).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Area>> GetAreasAsync()
            => await _readContext.Areas.ToArrayAsync();

        public Task AddAsync(Expert expert, IReadOnlyCollection<int> areas)
        {
            _editContext.Experts.AddAsync(expert);
            var expertAreas = areas.Select(s => new ExpertArea
            {
                Expert = expert,
                AreaId = (AreaType)s
            })
                                   .ToArray();

            _editContext.ExpertAreas.AddRange(expertAreas);
            return _editContext.SaveAsync();
        }

        public async Task UpdateAsync(Expert expert, IReadOnlyCollection<int> areas)
        {
            _editContext.Experts.Attach(expert).Property(e => e).IsModified = true;

            var expertAreas = await _editContext.ExpertAreas.Where(e => e.ExpertId == expert.UserId).ToArrayAsync();
            _editContext.ExpertAreas.RemoveRange(expertAreas);
            _editContext.ExpertAreas.AddRange(areas.Select(s => new ExpertArea
            {
                Expert = expert,
                AreaId = (AreaType)s
            }).ToArray());
            await _editContext.SaveAsync();
        }

        public async Task<PagingList<ExpertDetails>> GetAllDetailsAsync(int page, int pageSize)
        {
            var expertUsersQuery = (from expert in _readContext.Experts
                                    join user in _readContext.Users on expert.UserId equals user.Id
                                    select new { expert, user })
                                   .Skip(page * pageSize)
                                   .Take(pageSize);

            var expertAreas = await (from expertUser in expertUsersQuery
                                     join expertArea in _readContext.ExpertAreas on expertUser.user.Id equals expertArea.ExpertId
                                     join area in _readContext.Areas on expertArea.AreaId equals area.Id
                                     select new { expertArea.ExpertId, area }).ToArrayAsync();

            var lookUpAreas = expertAreas.ToLookup(k => k.ExpertId, v => v.area);

            var count = await _readContext.Experts.CountAsync();
            var expertUsers = await expertUsersQuery.ToArrayAsync();

            return new PagingList<ExpertDetails>(count, expertUsers.Select(expertUser => new ExpertDetails
            {
                About = expertUser.user.About,
                Address = expertUser.user.Address,
                Email = expertUser.user.Email,
                IsAvailable = expertUser.expert.IsAvailable,
                Name = expertUser.user.Name,
                Areas = lookUpAreas[expertUser.expert.UserId].ToArray()
            }));
        }
    }
}