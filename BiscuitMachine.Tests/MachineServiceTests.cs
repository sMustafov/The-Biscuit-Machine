using System;
using NUnit.Framework;
using BiscuitMachine.Models.Enums;
using BiscuitMachine.Services;

namespace BiscuitMachine.Tests
{
    class MachineServiceTests
    {
        public const int OVEN_TEMPERATURE = 220;

        public MachineService machineService;

        [SetUp]
        public void Setup()
        {
            machineService = new MachineService();
        }

        // Test machine switch on
        [Test]
        public void MachineSwitchOn()
        {
            machineService.SwitchOnMachine(OVEN_TEMPERATURE);

            Assert.AreEqual(MachineSwitch.On, machineService.GetMachineIsSwitchedOn());
        }

        // Test machine pause after switching on
        [Test]
        public void MachinePause()
        {
            machineService.SwitchOnMachine(OVEN_TEMPERATURE);
            Assert.AreEqual(MachineSwitch.On, machineService.GetMachineIsSwitchedOn());

            machineService.PauseMachine();
            Assert.AreEqual(MachineSwitch.Pause, machineService.GetMachineIsSwitchedOn());
        }

        // Test machine pause without switching on - it should throw exception
        [Test]
        public void MachineCannotBePauseWithoutSwitchingOn()
        {
            try
            {
                machineService.PauseMachine();

                // If it gets to this line, no exception was thrown
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The machine should be switched on to be paused!", ex.Message);
            }
        }

        // Test machine switch off with empty pot after switching on
        [Test]
        public void MachineSwitchOff()
        {
            machineService.SwitchOnMachine(OVEN_TEMPERATURE);
            Assert.AreEqual(MachineSwitch.On, machineService.GetMachineIsSwitchedOn());

            machineService.SwitchOffMachine();
            Assert.AreEqual(MachineSwitch.Off, machineService.GetMachineIsSwitchedOn());
        }

        // Test machine switch off with empty pot without switching on
        [Test]
        public void MachineCannotBeSwitchedOffWithoutSwitchingOn()
        {
            try
            {
                machineService.SwitchOffMachine();

                // If it gets to this line, no exception was thrown
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The machine should be switched on or paused to be switched off!", ex.Message);
            }
        }

        // Test machine switch off with empty pot after switching on and pausing it
        [Test]
        public void MachineSwitchOffAfterPausing()
        {
            machineService.SwitchOnMachine(OVEN_TEMPERATURE);
            Assert.AreEqual(MachineSwitch.On, machineService.GetMachineIsSwitchedOn());

            machineService.PauseMachine();
            Assert.AreEqual(MachineSwitch.Pause, machineService.GetMachineIsSwitchedOn());

            machineService.SwitchOffMachine();
            Assert.AreEqual(MachineSwitch.Off, machineService.GetMachineIsSwitchedOn());
        }
    }
}
