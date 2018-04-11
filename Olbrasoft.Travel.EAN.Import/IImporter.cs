using System;

namespace Olbrasoft.Travel.EAN.Import
{
    public interface IImporter : IDisposable
    {
        void Import(string path);
    }
}