using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vatsim.NET.Test
{
    [TestFixture]
    public class VatsimAPITest
    {
        [Test]
        [Category("EndToEnd")]       
        public async Task GetModuleTest()
        {
            IVatsim vatsim = await VatsimAPI.GetModule();

            Assert.That(vatsim, Is.Not.Null);

            Assert.That(vatsim.Data, Is.Not.Null);
            Assert.That(vatsim.GetMessages(), Is.Not.Null);
            Assert.That(await vatsim.GetMETAR("KCLE"), Is.Not.Null.Or.Empty);
        }
    }
}
