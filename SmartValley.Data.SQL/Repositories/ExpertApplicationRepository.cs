﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
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
            => await _readContext.ExpertApplications.Where(e => e.Status == status).ToArrayAsync();

        public Task<ExpertApplication> GetByIdAsync(long id)
            => Entities().FirstOrDefaultAsync(e => e.Id == id);

        public Task<ExpertApplication> GetByApplicantIdAsync(long userId)
            => Entities()
               .Where(e => e.ApplicantId == userId)
               .OrderByDescending(e => e.ApplyDate)
               .FirstOrDefaultAsync();

        public void Add(ExpertApplication expertApplication)
            => _editContext.ExpertApplications.Add(expertApplication);

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        private IQueryable<ExpertApplication> Entities()
            => _editContext.ExpertApplications.Include(x => x.ExpertApplicationAreas);
    }
}