using System;
using NUnit.Framework;
using BiscuitMachine.Models.Enums;
using BiscuitMachine.Services;

namespace BiscuitMachine.Tests
{
    class OvenServiceTests
    {
        public const int OVEN_TEMPERATURE = 220;
        public const int NOT_ALLOWED_TEMPERATURE = 210;

        public OvenService ovenService;

        [SetUp]
        public void Setup()
        {
            ovenService = new OvenService();
        }

        // Test oven switch on
        [Test]
        public void OvenSwitchOn()
        {
            ovenService.StartOven(OVEN_TEMPERATURE);

            Assert.AreEqual(MachineSwitch.On, ovenService.GetOvenIsSwitchedOn());
        }

        // Test oven switch on with temperature between 220 and 240
        [Test]
        public void OvenSwitchOnTemperature()
        {
            int expectedTemperature = 220;
            ovenService.StartOven(OVEN_TEMPERATURE);

            Assert.AreEqual(expectedTemperature, ovenService.GetOvenTemperature());
        }

        // Test oven switch on with not allowed temperature
        [Test]
        public void OvenSwitchOnTemperatureNotAllowed()
        {
            try
            {
                ovenService.StartOven(NOT_ALLOWED_TEMPERATURE);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("This temperature is not good for Biscuit! Please, give something between 220 and 240 degree!", ex.Message);
            }
        }

        // Test oven pause after switching on with allowed temperature
        [Test]
        public void OvenPauseAfterSwitchingOn()
        {
            ovenService.StartOven(OVEN_TEMPERATURE);

            ovenService.PauseOven();

            Assert.AreEqual(MachineSwitch.Pause, ovenService.GetOvenIsSwitchedOn());
        }

        // Test oven pause after switching on with temperature not changed
        [Test]
        public void OvenPauseAfterSwitchingOnTemperature()
        {
            int expectedTemperature = 220;

            ovenService.StartOven(OVEN_TEMPERATURE);

            ovenService.PauseOven();

            Assert.AreEqual(expectedTemperature, ovenService.GetOvenTemperature());
        }

        // Test oven pause without switching on
        [Test]
        public void OvenPauseWithoutSwitching()
        {
            try
            {
                ovenService.PauseOven();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The oven should be switched on to be paused!", ex.Message);
            }
        }

        // Test oven switch off after switching on
        [Test]
        public void OvenSwitchOffAfterSwitchingOn()
        {
            ovenService.StartOven(OVEN_TEMPERATURE);
            Assert.AreEqual(MachineSwitch.On, ovenService.GetOvenIsSwitchedOn());

            ovenService.StopOven();
            Assert.AreEqual(MachineSwitch.Off, ovenService.GetOvenIsSwitchedOn());
        }

        // Test oven's temperature of switch off after switching on
        [Test]
        public void OvenTemperatureSwitchOffAfterSwitchingOn()
        {
            int expectedTemperature = 0;

            ovenService.StartOven(OVEN_TEMPERATURE);

            ovenService.StopOven();

            Assert.AreEqual(expectedTemperature, ovenService.GetOvenTemperature());
        }

        // Test oven switch off without switching on
        [Test]
        public void OvenSwitchOffWithoutSwitchingOn()
        {
            try
            {
                ovenService.StopOven();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The oven should be switched on or paused to be switched off!", ex.Message);
            }
        }
    }
}
