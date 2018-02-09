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
    public class ExpertApplicationRepository : EntityCrudRepository<ExpertApplication>, IExpertApplicationRepository
    {
        public ExpertApplicationRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<int> AddAsync(ExpertApplication expertApplication, IReadOnlyCollection<int> areas)
        {
            EditContext.ExpertApplications.Add(expertApplication);
            EditContext.ExpertApplicationAreas.AddRange(areas.Select(area => new ExpertApplicationArea
                                                                             {
                                                                                 ExpertApplication = expertApplication,
                                                                                 ExpertiseAreaType = (ExpertiseAreaType) area
                                                                             }));

            return EditContext.SaveAsync();
        }

        public Task<bool> IsAppliedAsync(string address)
        {
            return (from explertApplication in ReadContext.ExpertApplications
                    join user in ReadContext.Users on explertApplication.ApplicantId equals user.Id
                    select user).AnyAsync(i => i.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
        }

        public Task<bool> IsConfirmedAsync(string address)
        {
            throw new System.NotImplementedException();
        }
    }
}