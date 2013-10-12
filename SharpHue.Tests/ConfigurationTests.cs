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

        [TestMethod]
        public void SetLightFromColor2()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            LightCollection lights = new LightCollection();

            new LightStateBuilder()
                .For(lights[1], lights[3])
                .TurnOn()
                .ColorTemperature(153)
                .Brightness(255)
                .Apply();
        }

        [TestMethod]
        public void SetAllLightStates()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");

            new LightStateBuilder()
                .ForAll()
                .TurnOn()
                .Brightness(255)
                .Effect(LightEffect.ColorLoop)
                .Apply();
        }

        [TestMethod]
        public void SetLightsRandom()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");

            LightCollection lights = new LightCollection();

            var indices = new int[] { 1, 3, 5 };

            foreach (var i in indices)
            {
                new LightStateBuilder()
                    .For(lights[i])
                    .TurnOn()
                    .Brightness(255)
                    .RandomColor()
                    .Apply();
            }
        }

        [TestMethod]
        public void FlashAllLights()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            new LightStateBuilder()
                .ForAll()
                .TurnOn()
                .Alert(LightAlert.LSelect)
                .Apply();
        }

        [TestMethod]
        public void ResetLights()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            LightCollection lights = new LightCollection();

            new LightStateBuilder()
                .For(lights, 1, 3)
                .TurnOn()
                .ColorTemperature(137)
                .Effect(LightEffect.None)
                .Apply();

            new LightStateBuilder()
                .For(lights[5])
                .TurnOff()
                .Apply();
        }

        [TestMethod]
        public void DiscoverNewLights()
        {
            Configuration.Initialize("36e02089265925772f085fcd3884ec9b");
            LightService.Discover();
        }
    }
}
