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
    public class ExpertApplicationRepository : IExpertApplicationRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ExpertApplicationRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public async Task<IReadOnlyCollection<ExpertApplication>> GetAllByStatusAsync(ExpertApplicationStatus status)
        {
            return await _readContext.ExpertApplications.Where(e => e.Status == status).ToArrayAsync();
        }

        public async Task<ExpertApplicationDetails> GetDetailsByIdAsync(long id)
        {
            var expertApplication = await _readContext.ExpertApplications.FirstOrDefaultAsync(e => e.Id == id);

            var areas = await (from expertApplicationArea in _readContext.ExpertApplicationAreas
                               join expertiseArea in _readContext.Areas on expertApplicationArea.AreaId equals expertiseArea.Id
                               where expertApplicationArea.ExpertApplicationId == id
                               select expertiseArea).ToArrayAsync();

            var applicant = await _readContext.Users.FirstAsync(user => user.Id == expertApplication.ApplicantId);

            return new ExpertApplicationDetails(applicant.Address, applicant.Email, expertApplication, areas);
        }

        public void Add(ExpertApplication expertApplication, IReadOnlyCollection<int> areas)
        {
            _editContext.ExpertApplications.Add(expertApplication);
            _editContext.ExpertApplicationAreas.AddRange(areas.Select(area => new ExpertApplicationArea
                                                                              {
                                                                                  ExpertApplication = expertApplication,
                                                                                  AreaId = (AreaType) area
                                                                              }));
        }

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        public void SetAccepted(ExpertApplicationDetails applicationDetails, List<int> areas)
        {
            applicationDetails.ExpertApplication.Status = ExpertApplicationStatus.Accepted;

            var applicationExpertAreas = applicationDetails.Areas.Select(s => new ExpertApplicationArea
                                                                              {
                                                                                  ExpertApplicationId = applicationDetails.ExpertApplication.Id,
                                                                                  AreaId = s.Id
                                                                              })
                                                           .ToArray();

            _editContext.ExpertApplicationAreas.AttachRange(applicationExpertAreas);

            foreach (var applicationExpertArea in applicationExpertAreas)
            {
                applicationExpertArea.Status = ExpertApplicationStatus.Rejected;
                if (areas.Contains((int) applicationExpertArea.AreaId))
                    applicationExpertArea.Status = ExpertApplicationStatus.Accepted;
            }

            _editContext.ExpertApplications.Update(applicationDetails.ExpertApplication);
            _editContext.ExpertApplicationAreas.UpdateRange(applicationExpertAreas);
        }

        public void SetRejected(ExpertApplicationDetails applicationDetails)
        {
            applicationDetails.ExpertApplication.Status = ExpertApplicationStatus.Rejected;

            var applicationExpertAreas = applicationDetails.Areas.Select(s => new ExpertApplicationArea
                                                                              {
                                                                                  ExpertApplicationId = applicationDetails.ExpertApplication.Id,
                                                                                  AreaId = s.Id,
                                                                                  Status = ExpertApplicationStatus.Rejected
                                                                              })
                                                           .ToList();

            _editContext.ExpertApplicationAreas.AttachRange(applicationExpertAreas);
            _editContext.ExpertApplications.Update(applicationDetails.ExpertApplication);
            _editContext.ExpertApplicationAreas.UpdateRange(applicationExpertAreas);
        }

        public async Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address)
        {
            var existExpertApplitacion = await (from explertApplication in _readContext.ExpertApplications
                                                join user in _readContext.Users on explertApplication.ApplicantId equals user.Id
                                                where user.Address == address
                                                orderby explertApplication.ApplyDate descending
                                                select explertApplication).FirstOrDefaultAsync();

            return existExpertApplitacion?.Status ?? ExpertApplicationStatus.None;
        }
    }
}