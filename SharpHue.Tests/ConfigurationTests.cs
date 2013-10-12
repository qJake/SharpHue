using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

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

        [TestMethod]
        [ExpectedException(typeof(HueConfigurationException))]
        public void GetLightsNoAuth()
        {
            LightCollection lights = new LightCollection();
        }

        [TestMethod]
        public void SetLightFromColor()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            LightCollection lights = new LightCollection();

            LightStateBuilder b = new LightStateBuilder()
                .TurnOff()
                .Brightness(255)
                .XYCoordinates(0.25, 0.725);

            lights[2].SetState(b);
        }
    }
}
