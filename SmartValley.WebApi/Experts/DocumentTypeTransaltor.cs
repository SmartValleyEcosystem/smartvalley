using System;

namespace SmartValley.WebApi.Experts
{
    public static class DocumentTypeTransaltor
    {
        public static Domain.Entities.DocumentType ToDomain(this DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.Id:
                    return Domain.Entities.DocumentType.Id;
                case DocumentType.DriverLicense:
                    return Domain.Entities.DocumentType.DriverLicense;
                case DocumentType.Other:
                    return Domain.Entities.DocumentType.Other;
                case DocumentType.Passport:
                    return Domain.Entities.DocumentType.Passport;
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null);
            }
        }

        public static DocumentType FromDomain(this Domain.Entities.DocumentType documentType)
        {
            switch (documentType)
            {
                case Domain.Entities.DocumentType.Id:
                    return DocumentType.Id;
                case Domain.Entities.DocumentType.DriverLicense:
                    return DocumentType.DriverLicense;
                case Domain.Entities.DocumentType.Other:
                    return DocumentType.Other;
                case Domain.Entities.DocumentType.Passport:
                    return DocumentType.Passport;
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null);
            }
        }
    }
}