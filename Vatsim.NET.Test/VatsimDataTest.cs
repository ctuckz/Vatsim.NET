using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vatsim.NET.Test
{
    [TestFixture]
    public class VatsimDataTest
    {
        [Test]
        [Category("EndToEnd")]
        public async Task General__LoadDataTest()
        {
            VatsimStatus status = new VatsimStatus(new VatsimStatusLoader());
            await status.Initialize();
            VatsimData data = new VatsimData(new VatsimDataLoader(), status);

            DateTime utcNow = DateTime.UtcNow;

            GeneralData general = await data.GetGeneralData();

            Assert.That(general, Is.Not.Null);
            Assert.That(general.AtisRefreshInterval, Is.GreaterThan(0));
            Assert.That(general.LastUpdateUtc, Is.LessThan(utcNow));
            Assert.That(general.NumConnectedClients, Is.GreaterThan(0));
            Assert.That(general.ReloadUtc, Is.GreaterThan(utcNow));
            Assert.That(general.Version, Is.Not.Null.Or.Empty);
        }
    }
}
