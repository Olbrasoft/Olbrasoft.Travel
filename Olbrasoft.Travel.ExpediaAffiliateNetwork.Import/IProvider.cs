using System;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    public interface IProvider
    {
        event EventHandler<string[]> SplittingLine;
        void ReadToEnd(string path);
        string GetFirstLine(string path);
    }
}
