using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpHue.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void GetLights()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            LightCollection lights = new LightCollection();
            Assert.IsTrue(lights.Count > 0);
        }
    }
}
