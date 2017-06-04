using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal interface IVatsimStatusLoader
    {
        Task<string> LoadData(string statusUri);
    }
}
