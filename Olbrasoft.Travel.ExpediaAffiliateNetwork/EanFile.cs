using System;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork
{
    public class EanFile
    {
        public bool IsMultilanguage => GetIsMultilanguage(DownloadUrl);
        
        public EanFile(Uri downloadUrl, TypeOfEanFile typeOfEanFile, Type assignedEntityType)
        {
            DownloadUrl = downloadUrl;
            TypeOfEanFile = typeOfEanFile;
            AssignedEntityType = assignedEntityType;
        }

        public Uri DownloadUrl { get; }

        public TypeOfEanFile TypeOfEanFile { get; }

        public Type AssignedEntityType { get; }


        public static bool GetIsMultilanguage(Uri uri)
        {          
           var filename = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
           return filename.EndsWith("xx_XX");
        }
    }
}