﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.EAN.Import
{
   public interface IImport<in T>
    {
        void Import(string path);
        void ImportBatch(T[] eanEntities);
    }
}
