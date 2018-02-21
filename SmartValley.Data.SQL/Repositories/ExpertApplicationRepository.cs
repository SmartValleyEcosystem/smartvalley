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
    public class ExpertApplicationRepository : EntityCrudRepository<ExpertApplication>, IExpertApplicationRepository
    {
        public ExpertApplicationRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<ExpertApplication>> GetAllByStatusAsync(ExpertApplicationStatus status)
        {
            return await ReadContext.ExpertApplications.Where(e => e.Status == status).ToArrayAsync();
        }

        public async Task<ExpertApplicationDetails> GetDetailsByIdAsync(long id)
        {
            var expertApplication = await ReadContext.ExpertApplications.FirstOrDefaultAsync(e => e.Id == id);

            var areas = await (from expertApplicationArea in ReadContext.ExpertApplicationAreas.Where(e => e.ExpertApplicationId == id)
                               join expertiseArea in ReadContext.Areas on expertApplicationArea.AreaId equals expertiseArea.Id
                               select expertiseArea).ToArrayAsync();

            return new ExpertApplicationDetails
            {
                ExpertApplication = expertApplication,
                Areas = areas
            };
        }

        public Task<int> AddAsync(ExpertApplication expertApplication, IReadOnlyCollection<int> areas)
        {
            EditContext.ExpertApplications.Add(expertApplication);
            EditContext.ExpertApplicationAreas.AddRange(areas.Select(area => new ExpertApplicationArea
            {
                ExpertApplication = expertApplication,
                AreaId = (AreaType)area
            }));

            return EditContext.SaveAsync();
        }

        public Task SetAcceptedAsync(ExpertApplicationDetails applicationDetails, List<int> areas)
        {
            applicationDetails.ExpertApplication.Status = ExpertApplicationStatus.Accepted;

            var applicationExpertAreas = applicationDetails.Areas.Select(s => new ExpertApplicationArea
            {
                ExpertApplicationId = applicationDetails.ExpertApplication.Id,
                AreaId = s.Id
            })
                                                           .ToArray();

            EditContext.ExpertApplicationAreas.AttachRange(applicationExpertAreas);

            foreach (var applicationExpertArea in applicationExpertAreas)
            {
                applicationExpertArea.Status = ExpertApplicationStatus.Rejected;
                if (areas.Contains((int)applicationExpertArea.AreaId))
                {
                    applicationExpertArea.Status = ExpertApplicationStatus.Accepted;
                }
            }

            EditContext.ExpertApplications.Update(applicationDetails.ExpertApplication);
            EditContext.ExpertApplicationAreas.UpdateRange(applicationExpertAreas);
            return EditContext.SaveAsync();
        }

        public Task SetRejectedAsync(ExpertApplicationDetails applicationDetails)
        {
            applicationDetails.ExpertApplication.Status = ExpertApplicationStatus.Rejected;

            var applicationExpertAreas = applicationDetails.Areas.Select(s => new ExpertApplicationArea
            {
                ExpertApplicationId = applicationDetails.ExpertApplication.Id,
                AreaId = s.Id,
                Status = ExpertApplicationStatus.Rejected
            })
                                                           .ToList();

            EditContext.ExpertApplicationAreas.AttachRange(applicationExpertAreas);
            EditContext.ExpertApplications.Update(applicationDetails.ExpertApplication);
            EditContext.ExpertApplicationAreas.UpdateRange(applicationExpertAreas);
            return EditContext.SaveAsync();
        }

        public async Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(string address)
        {
            var existExpertApplitacion = await (from explertApplication in ReadContext.ExpertApplications
                                                join user in ReadContext.Users on explertApplication.ApplicantId equals user.Id
                                                where user.Address.Equals(address, StringComparison.OrdinalIgnoreCase)
                                                orderby explertApplication.ApplyDate
                                                select explertApplication).FirstOrDefaultAsync();

            return existExpertApplitacion?.Status ?? ExpertApplicationStatus.None;
        }
    }
}