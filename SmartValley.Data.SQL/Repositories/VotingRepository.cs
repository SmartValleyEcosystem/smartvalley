using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class VotingRepository : EntityCrudRepository<Voting>, IVotingRepository
    {
        public VotingRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<Voting>> GetAllTillDateAsync(DateTimeOffset tillDate)
        {
            return await ReadContext.Votings.Where(i => i.EndDate <= tillDate).ToArrayAsync();
        }
    }
}