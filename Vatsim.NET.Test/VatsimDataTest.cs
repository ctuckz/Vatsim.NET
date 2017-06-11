using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Vatsim.NET.Test
{
    [TestFixture]
    public class VatsimDataTest
    {
        private static readonly string TestDataDir = Path.Combine(
            Path.GetDirectoryName(typeof(VatsimDataTest).GetTypeInfo().Assembly.Location), "VatsimDataTestData");

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

        [Test]
        public async Task General__ReadsGeneralSection()
        {
            VatsimData data = getMockData(File.ReadAllText(Path.Combine(TestDataDir, "FullTestData.txt")));

            DateTime utcNow = DateTime.UtcNow;

            GeneralData general = await data.GetGeneralData();

            Assert.That(general, Is.Not.Null);
            Assert.That(general.AtisRefreshInterval, Is.EqualTo(5));
            Assert.That(general.LastUpdateUtc, Is.EqualTo(new DateTime(2017, 6, 10, 21, 47, 15)));
            Assert.That(general.NumConnectedClients, Is.EqualTo(595));
            Assert.That(general.ReloadUtc, Is.EqualTo(utcNow.AddMinutes(2)).Within(TimeSpan.FromSeconds(5)));
            Assert.That(general.Version, Is.EqualTo("8"));
        }

        [Test]
        [TestCase("blahblah")]
        [TestCase("")]
        public void General__MissingGeneralSection_Throws(string inputData)
        {
            VatsimData data = getMockData(inputData);

            // Assert.That bubbles up the exception instead of asserting that it's thrown. Have to use ThrowsAsync instead.
            Assert.ThrowsAsync<Exception>(async () => await data.GetGeneralData());
        }

        [Test]
        public async Task General__EmptyGeneralSection_LoadsWithDefaults()
        {
            VatsimData data = getMockData("!GENERAL:");

            GeneralData general = await data.GetGeneralData();

            Assert.That(general, Is.Not.Null);
            Assert.That(general.AtisRefreshInterval, Is.EqualTo(0));
            Assert.That(general.LastUpdateUtc, Is.EqualTo(default(DateTime)));
            Assert.That(general.NumConnectedClients, Is.EqualTo(0));
            Assert.That(general.ReloadUtc, Is.EqualTo(default(DateTime)));
            Assert.That(general.Version, Is.Empty);
        }

        [Test]
        public async Task General__EmptyGeneralValues_LoadsWithDefaults()
        {
            VatsimData data = getMockData("!GENERAL:\nVersion = \nRELOAD = \nUPDATE = \nATIS ALLOW MIN = \nCONNECTED CLIENTS = ");

            GeneralData general = await data.GetGeneralData();

            Assert.That(general, Is.Not.Null);
            Assert.That(general.AtisRefreshInterval, Is.EqualTo(0));
            Assert.That(general.LastUpdateUtc, Is.EqualTo(default(DateTime)));
            Assert.That(general.NumConnectedClients, Is.EqualTo(0));
            Assert.That(general.ReloadUtc, Is.EqualTo(default(DateTime)));
            Assert.That(general.Version, Is.Empty);
        }

        [Test]
        public async Task General__UnderRefreshTime_DoesNotRefresh()
        {
            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetDataUrl()).Returns(new Uri("http://localhost"));

            Mock<IVatsimDataLoader> loader = new Mock<IVatsimDataLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                .ReturnsAsync("!GENERAL:\nVERSION = 1\nRELOAD = 5");

            VatsimData data = new VatsimData(loader.Object, status.Object);

            await data.GetGeneralData();
            loader.Verify(l => l.LoadData(It.IsAny<string>()), Times.Once());

            Thread.Sleep(100);

            await data.GetGeneralData();
            loader.Verify(l => l.LoadData(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task General__AfterRefreshTime_RefreshesData()
        {
            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetDataUrl()).Returns(new Uri("http://localhost"));

            Mock<IVatsimDataLoader> loader = new Mock<IVatsimDataLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                .ReturnsAsync("!GENERAL:\nVERSION = 1\nRELOAD = 0");

            VatsimData data = new VatsimData(loader.Object, status.Object);

            await data.GetGeneralData();
            loader.Verify(l => l.LoadData(It.IsAny<string>()), Times.Once());

            Thread.Sleep(100);

            await data.GetGeneralData();
            loader.Verify(l => l.LoadData(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        [Category("EndToEnd")]
        public async Task Clients__LoadDataTest()
        {
            VatsimStatus status = new VatsimStatus(new VatsimStatusLoader());
            await status.Initialize();
            VatsimData data = new VatsimData(new VatsimDataLoader(), status);

            IReadOnlyList<ClientData> clients = await data.GetClientData();

            Assert.That(clients, Is.Not.Null);
            Assert.That(clients, Has.Count.EqualTo((await data.GetGeneralData()).NumConnectedClients));
        }

        [Test]
        public async Task Clients__ReadsPilotData()
        {
            VatsimData data = getMockData(File.ReadAllText(Path.Combine(TestDataDir, "FullTestData.txt")));

            IReadOnlyList<ClientData> clients = await data.GetClientData();

            Assert.That(clients, Is.Not.Null);

            ClientData client = clients[0];
            Assert.That(client.Pilot, Is.Not.Null);
            Assert.That(client.Pilot.Callsign, Is.EqualTo("85663"));
            Assert.That(client.Pilot.ClientID, Is.EqualTo(910794));
            Assert.That(client.Pilot.ClientType, Is.EqualTo("PILOT"));
            Assert.That(client.Pilot.Frequency, Is.EqualTo(0));
            Assert.That(client.Pilot.LastAtisRecieved, Is.Null);
            Assert.That(client.Pilot.LogOnTime, Is.EqualTo("20170610201201".ToDateTime()));
            Assert.That(client.Pilot.Rating, Is.EqualTo("1"));
            Assert.That(client.Pilot.RealName, Is.EqualTo("VASSILI GRAMOLIN USSS"));
            Assert.That(client.Pilot.Server, Is.EqualTo("CZECH"));
        }

        [Test]
        public async Task Clients__ReadsPositionData()
        {
            VatsimData data = getMockData(File.ReadAllText(Path.Combine(TestDataDir, "FullTestData.txt")));

            IReadOnlyList<ClientData> clients = await data.GetClientData();

            Assert.That(clients, Is.Not.Null);

            ClientData client = clients[2];
            Assert.That(client.Position, Is.Not.Null);
            Assert.That(client.Position.AltimeterInHg, Is.EqualTo(29.88));
            Assert.That(client.Position.AltimeterMb, Is.EqualTo(1011));
            Assert.That(client.Position.Altitude, Is.EqualTo(10415));
            Assert.That(client.Position.GroundSpeed, Is.EqualTo(274));
            Assert.That(client.Position.Heading, Is.EqualTo(137));
            Assert.That(client.Position.Latitude, Is.EqualTo(21.50482));
            Assert.That(client.Position.Longitude, Is.EqualTo(-87.43398));
            Assert.That(client.Position.Transponder, Is.EqualTo(502));
        }

        private static VatsimData getMockData(string testData)
        {
            Mock<IVatsimStatus> status = new Mock<IVatsimStatus>(MockBehavior.Strict);
            status.Setup(s => s.GetDataUrl()).Returns(new Uri("http://localhost"));

            Mock<IVatsimDataLoader> loader = new Mock<IVatsimDataLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                .ReturnsAsync(testData);

            return new VatsimData(loader.Object, status.Object);
        }
    }
}
