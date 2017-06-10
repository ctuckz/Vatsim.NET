using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

namespace Vatsim.NET.Test
{
    [TestFixture]
    public class VatsimStatusTest
    {
        [Test]
        [Category("EndToEnd")]
        public async Task LoadDataTest()
        {
            string[] expectedDataUrls = new[] { "http://info.vroute.net/vatsim-data.txt",
                                                "http://data.vattastic.com/vatsim-data.txt",
                                                "http://vatsim-data.hardern.net/vatsim-data.txt",
                                                "http://wazzup.openaviationdata.com/vatsim/vatsim-data.txt" };
            string[] expectedServerUrls = new[] { "http://info.vroute.net/vatsim-servers.txt",
                                                   "http://data.vattastic.com/vatsim-servers.txt",
                                                   "http://vatsim-data.hardern.net/vatsim-servers.txt",
                                                   "http://wazzup.openaviationdata.com/vatsim/vatsim-servers.txt" };
            string[] expectedMetarUrls = new[] { "http://metar.vatsim.net/metar.php" };
            string[] expectedAtisUrls = new[] { "http://stats.vatsim.net/atis.html" };
            string[] expectedUserUrls = new[] { "http://stats.vatsim.net/search_id.php" };

            Mock<IVatsimStatusLoader> loader = new Mock<IVatsimStatusLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                  .ReturnsAsync(NormalData);

            VatsimStatus status = new VatsimStatus(loader.Object);
            await status.Initialize();

            Assert.That(status.GetAllDataUrls().Select(u => u.ToString()), Is.EquivalentTo(expectedDataUrls));
            Assert.That(status.GetAllServerUrls().Select(u => u.ToString()), Is.EquivalentTo(expectedServerUrls));
            Assert.That(status.GetAllMetarUrls().Select(u => u.ToString()), Is.EquivalentTo(expectedMetarUrls));
            Assert.That(status.GetAllAtisUrls().Select(u => u.ToString()), Is.EquivalentTo(expectedAtisUrls));
            Assert.That(status.GetAllUserUrls().Select(u => u.ToString()), Is.EquivalentTo(expectedUserUrls));
            Assert.That(status.GetMessages(), Is.Empty);
        }

        [Test]
        public async Task LoadData__MoveTo()
        {
            Mock<IVatsimStatusLoader> loader = new Mock<IVatsimStatusLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData("http://status.vatsim.net/status.txt"))
                  .ReturnsAsync(MoveToData);
            loader.Setup(l => l.LoadData("http://MoveTo"))
                  .ReturnsAsync(NormalData);

            VatsimStatus status = new VatsimStatus(loader.Object);
            await status.Initialize();

            loader.Verify(l => l.LoadData("http://status.vatsim.net/status.txt"), Times.Once());
            loader.Verify(l => l.LoadData("http://MoveTo"), Times.Once());
        }

        [Test]
        public async Task LoadData__Messages()
        {
            string[] expected = new[] { "Message1", "Message2" };

            Mock<IVatsimStatusLoader> loader = new Mock<IVatsimStatusLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                  .ReturnsAsync(MessageData);

            VatsimStatus status = new VatsimStatus(loader.Object);
            await status.Initialize();

            Assert.That(status.GetMessages(), Is.EquivalentTo(expected));
        }

        [Test]
        public async Task LoadData__EmptyData()
        {
            Mock<IVatsimStatusLoader> loader = new Mock<IVatsimStatusLoader>(MockBehavior.Strict);
            loader.Setup(l => l.LoadData(It.IsAny<string>()))
                  .ReturnsAsync(string.Empty);

            VatsimStatus status = new VatsimStatus(loader.Object);
            await status.Initialize();

            Assert.That(status.GetAllAtisUrls(), Is.Empty);
            Assert.That(status.GetAllDataUrls(), Is.Empty);
            Assert.That(status.GetAllMetarUrls(), Is.Empty);
            Assert.That(status.GetAllServerUrls(), Is.Empty);
            Assert.That(status.GetAllUserUrls(), Is.Empty);
            Assert.That(status.GetMessages(), Is.Empty);
        }

        #region Test Data

        private const string NormalData =
        #region Normal Data
            @"; IMPORTANT NOTE: to use less bandwidth, please download this file ONE TIME ONLY when
;                 your application starts, and load it locally
;
; Data formats are:
;
; 120128:NOTCP - used by WhazzUp only
; msg0         - message to be displayed at application startup
; url0         - URLs where complete data files are available. Please choose one randomly every time
; url1         - URLs where servers list data files are available. Please choose one randomly every time
; moveto0      - URL where to retrieve a more updated status.txt file that overrides this one
; metar0       - URL where to retrieve metar. Invoke it passing a parameter like for example: http://data.satita.net/metar.html?id=KBOS
; atis0        - URL where to retrieve atis. Invoke it passing a parameter like for example: http://data.satita.net/atis.html?callsign=BOS_CTR
;                WARNING: please don't abuse it. Requests take network bandwidth. If possibile please use atis that are present into satnet-data.txt file
; user0        - URL where to retrieve statistics web pages
;
;
120218:NOTCP
;
url0=http://info.vroute.net/vatsim-data.txt
url0=http://data.vattastic.com/vatsim-data.txt
url0=http://vatsim-data.hardern.net/vatsim-data.txt
url0=http://wazzup.openaviationdata.com/vatsim/vatsim-data.txt
;
url1=http://info.vroute.net/vatsim-servers.txt
url1=http://data.vattastic.com/vatsim-servers.txt
url1=http://vatsim-data.hardern.net/vatsim-servers.txt
url1=http://wazzup.openaviationdata.com/vatsim/vatsim-servers.txt
;
metar0=http://metar.vatsim.net/metar.php
;
atis0=http://stats.vatsim.net/atis.html
;
user0=http://stats.vatsim.net/search_id.php
;
; END
";
        #endregion

        private const string MoveToData = "moveto0=http://MoveTo";

        private const string MessageData = @"msg0=Message1
msg0=Message2";

        #endregion
    }
}
