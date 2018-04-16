﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Estimates.Requests;
using SmartValley.WebApi.Estimates.Responses;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly EthereumClient _ethereumClient;
        private readonly IEstimationService _estimationService;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;

        public EstimatesController(
            EthereumClient ethereumClient,
            IEstimationService estimationService,
            IScoringCriterionRepository scoringCriterionRepository)
        {
            _ethereumClient = ethereumClient;
            _estimationService = estimationService;
            _scoringCriterionRepository = scoringCriterionRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SubmitEstimatesRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _estimationService.SubmitEstimatesAsync(User.GetUserId(), request);
            return NoContent();
        }

        [HttpGet]
        public async Task<CollectionResponse<ScoringStatisticsInAreaResponse>> GetEstimatesAsync(long projectId)
        {
            var scoringStatistics = await _estimationService.GetScoringStatisticsAsync(projectId);
            return new CollectionResponse<ScoringStatisticsInAreaResponse>
                   {
                       Items = scoringStatistics.Select(ScoringStatisticsInAreaResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [Route("criteria")]
        public async Task<CollectionResponse<AreaScoringCriteriaResponse>> GetCriteriaAsync()
        {
            var criteria = await _scoringCriterionRepository.GetAllAsync();
            return new CollectionResponse<AreaScoringCriteriaResponse>
                   {
                       Items = criteria
                               .GroupBy(c => c.AreaType)
                               .Select(g => AreaScoringCriteriaResponse.Create(g.Key.FromDomain(), g.ToArray()))
                               .ToArray()
                   };
        }
    }
}