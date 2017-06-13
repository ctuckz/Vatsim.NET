using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Vatsim.NET.Test
{
    [TestFixture]
    [TestOf(typeof(Vatsim))]
    public class VatsimTest
    {
        [Test]
        public async Task GetMETARTest()
        {
            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetMetarUrl()).Returns(new Uri("http://localhost"));

            Mock<IMetarDataLoader> loader = new Mock<IMetarDataLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("blahblah");

            Vatsim vatsim = new Vatsim(status.Object, Mock.Of<IVatsimData>(), loader.Object);

            string metar = await vatsim.GetMETAR("abc");

            Assert.That(metar, Is.EqualTo("blahblah"));
        }

        [Test]
        [TestCase("No METAR available for")]
        [TestCase("")]
        [TestCase(null)]
        public void GetMETAR__InvalidICAO_Throws(string response)
        {
            string icao = "abc";

            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetMetarUrl()).Returns(new Uri("http://localhost"));

            Mock<IMetarDataLoader> loader = new Mock<IMetarDataLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>(), icao))
                .ReturnsAsync(response);

            Vatsim vatsim = new Vatsim(status.Object, Mock.Of<IVatsimData>(), loader.Object);

            Assert.ThrowsAsync<Exception>(async () => await vatsim.GetMETAR(icao));
        }

        [Test]
        public void GetMETAR__NullICAO_Throws()
        {
            Vatsim vatsim = new Vatsim(Mock.Of<IVatsimStatus>(), Mock.Of<IVatsimData>(), Mock.Of<IMetarDataLoader>());

            Assert.ThrowsAsync<ArgumentNullException>(async () => await vatsim.GetMETAR(null));
        }

        [Test]
        public void GetMETAR__NoMETARUrl_Throws()
        {
            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetMetarUrl()).Returns(() => null);

            Vatsim vatsim = new Vatsim(status.Object, Mock.Of<IVatsimData>(), Mock.Of<IMetarDataLoader>());

            Assert.ThrowsAsync<Exception>(async () => await vatsim.GetMETAR("abc"));
        }
    }
}
