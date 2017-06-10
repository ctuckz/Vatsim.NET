using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    public static class VatsimAPI
    {
        private static IVatsim _vatsim;

        public static IVatsim GetModule()
        {
            VatsimStatusLoader statusLoader = new VatsimStatusLoader();
            VatsimStatus status = new VatsimStatus(statusLoader);

            VatsimDataLoader dataLoader = new VatsimDataLoader();
            VatsimData data = new VatsimData(dataLoader, status);

            return _vatsim ?? (_vatsim = new Vatsim(status, data));
        }
    }
}
