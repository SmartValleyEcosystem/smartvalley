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

        public Task<IReadOnlyCollection<Project>> GetProjectsForScoringByCategoryAsync(ScoringCategory category)
        {
            return _projectRepository.GetAllByCategoryAsync(category);
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address)
        {
            return _projectRepository.GetAllByAuthorAddressAsync(address);
        }
    }
}