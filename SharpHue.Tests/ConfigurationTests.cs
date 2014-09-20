using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void GetConfiguration()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            var config = Configuration.GetBridgeConfiguration();

            Assert.IsNotNull(config.BridgeName);
            Assert.IsTrue(config.Whitelist.Count > 0);
        }

        [TestMethod]
        public void DeleteWhitelistEntry()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            var config = Configuration.GetBridgeConfiguration();

            var result = Configuration.DeleteUser(config.Whitelist.Keys.First(k => k == "ffffffff89f6dab073d6adc373d6adc3"));

            Assert.IsTrue(result);
        }
    }
}
