using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal interface IVatsimDataLoader
    {
        Task<string> LoadData(string dataUrl);
    }
}
