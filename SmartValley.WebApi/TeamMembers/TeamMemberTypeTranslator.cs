using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.TeamMembers
{
    public static class TeamMemberTypeTranslator
    {
        public static Type FromDomain(this TeamMemberType type)
        {
            switch (type)
            {
                case TeamMemberType.CEO:
                    return Type.CEO;
                case TeamMemberType.CFO:
                    return Type.CFO;
                case TeamMemberType.CMO:
                    return Type.CMO;
                case TeamMemberType.CTO:
                    return Type.CTO;
                case TeamMemberType.PR:
                    return Type.PR;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}