using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    public static class VatsimAPI
    {
        private static IVatsim _vatsim;

        public static async Task<IVatsim> GetModule()
        {
            VatsimStatusLoader statusLoader = new VatsimStatusLoader();
            VatsimStatus status = new VatsimStatus(statusLoader);
            await status.Initialize();

            VatsimDataLoader dataLoader = new VatsimDataLoader();
            VatsimData data = new VatsimData(dataLoader, status);

            MetarDataLoader metarLoader = new MetarDataLoader();

            return _vatsim ?? (_vatsim = new Vatsim(status, data, metarLoader));
        }
    }
}
