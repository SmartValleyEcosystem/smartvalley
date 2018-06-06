using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class Expert
    {
        public Expert(long userId, bool isAvailable, bool isInHouse = false)
        {
            UserId = userId;
            IsAvailable = isAvailable;
            IsInHouse = isInHouse;
            ExpertAreas = new List<ExpertArea>();
        }

        public long UserId { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsInHouse { get; set; }

        public User User { get; set; }

        public ICollection<ExpertArea> ExpertAreas { get; set; }

        public void SetAreas(IReadOnlyCollection<int> areas)
        {
            ExpertAreas.Clear();

            foreach (var area in areas)
            {
                ExpertAreas.Add(new ExpertArea {AreaId = (AreaType) area});
            }
        }
    }
}