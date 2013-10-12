using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpHue.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void DeviceDiscovery()
        {
            Configuration.Initialize();
        }
    }
}
