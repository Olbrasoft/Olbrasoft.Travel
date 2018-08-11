using System;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    public interface IImporter : IDisposable
    {
        void Import(string path);
    }
}