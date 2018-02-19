using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
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

        public Task<Expert> GetByEmailAsync(string email)
        {
            return (from expert in _readContext.Experts
                    join user in _readContext.Users on expert.UserId equals user.Id
                    where user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                    select expert).FirstOrDefaultAsync();
        }

        public Task<int> UpdateWholeAsync(Expert expert)
        {
            _editContext.Experts.Attach(expert);
            _editContext.Entity(expert).State = EntityState.Modified;
            return _editContext.SaveAsync();
        }

        public Task AddAsync(Expert expert)
        {
            _editContext.Experts.AddAsync(expert);
            return _editContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<Expert>> GetAllAsync()
            => await _readContext.Experts.ToArrayAsync();

        public async Task<IReadOnlyCollection<ExpertDetails>> GetAllDetailsAsync()
        {
            var expertAreas = await (from expertArea in _readContext.ExpertAreas
                                     join area in _readContext.Areas on expertArea.AreaId equals area.Id
                                     select new { expertArea.ExpertId, area }).ToArrayAsync();

            var expertUsers = await (from expert in _readContext.Experts
                                     join user in _readContext.Users on expert.UserId equals user.Id
                                     select new { expert, user }).ToArrayAsync();

            var lookUpAreas = expertAreas.ToLookup(k => k.ExpertId, v => v.area);

            return expertUsers.Select(expertUser => new ExpertDetails
            {
                                                        About = expertUser.user.About,
                                                        Address = expertUser.user.Address,
                                                        Email = expertUser.user.Email,
                                                        IsAvailable = expertUser.expert.IsAvailable,
                                                        Name = expertUser.user.Name,
                                                        Areas = lookUpAreas[expertUser.expert.UserId].ToArray()
                                                    })
                              .ToArray();
        }
    }
}
