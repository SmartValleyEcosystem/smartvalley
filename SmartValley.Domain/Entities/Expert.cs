using System;
using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class Expert
    {
        // ReSharper disable once UnusedMember.Local
        private Expert()
        {
        }

        public Expert(long userId, bool isAvailable, string about = null) : this(userId, isAvailable, new int[0], about)
        {

        }

        public Expert(long userId, bool isAvailable, IReadOnlyCollection<int> areas, string about = null)
        {
            if (areas == null)
                throw new ArgumentNullException(nameof(areas));

            UserId = userId;
            IsAvailable = isAvailable;
            About = about;
            ExpertAreas = new List<ExpertArea>();

            SetAreas(areas);
        }

        public long UserId { get; private set; }

        public bool IsAvailable { get; set; }

        public string About { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public User User { get; private set; }

        public ICollection<ExpertArea> ExpertAreas { get; private set; }

        public void SetAreas(IReadOnlyCollection<int> areas)
        {
            ExpertAreas.Clear();

            foreach (var area in areas)
            {
                ExpertAreas.Add(new ExpertArea { AreaId = (AreaType)area });
            }
        }
    }
}