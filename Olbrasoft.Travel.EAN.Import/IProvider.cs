using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.EAN.Import
{
    public interface IProvider
    {
        event EventHandler<string[]> SplittingLine;
        void ReadToEnd(string path);
    }
}
