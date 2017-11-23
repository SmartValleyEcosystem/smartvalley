using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Exceptions;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;

        public ScoringService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetProjectsForScoringByCategory(ScoringCategory requestScoringCategory)
        {
            var projects = await _projectRepository.GetAllAsync();

            switch (requestScoringCategory)

            {
                case ScoringCategory.Unknown:
                    return new List<Project>();
                case ScoringCategory.Hr:
                    return projects.Where(p => p.HrEstimatesCount < 3);
                case ScoringCategory.Analyst:
                    return projects.Where(p => p.AnalystEstimatesCount < 3);
                case ScoringCategory.Tech:
                    return projects.Where(p => p.TechnicalEstimatesCount < 3);
                case ScoringCategory.Lawyer:
                    return projects.Where(p => p.LawyerEstimatesCount < 3);
                    default:
                     throw new AppErrorException(ErrorCode.InvalidScroringCategory);
            }
        }

        public Task<IEnumerable<Project>> GetProjectsByAddress(string address)
        {
            return _projectRepository.GetAllByAuthorAddressAsync(address);
        }
    }
}