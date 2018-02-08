using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}