using System;

namespace Olbrasoft.Travel.EAN
{
    public class EanFile
    {
        public EanFile(Uri downloadUrl, TypeOfEanFile typeOfEanFile, Type assignedEntityType)
        {
            DownloadUrl = downloadUrl;
            TypeOfEanFile = typeOfEanFile;
            AssignedEntityType = assignedEntityType;
        }

        public Uri DownloadUrl { get; }

        public TypeOfEanFile TypeOfEanFile { get; }

        public Type AssignedEntityType { get; }



    }
}