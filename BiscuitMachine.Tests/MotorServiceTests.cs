using System;
using NUnit.Framework;
using BiscuitMachine.Models;
using BiscuitMachine.Models.Enums;
using BiscuitMachine.Services;

namespace BiscuitMachine.Tests
{
    class MotorServiceTests
    {
        public const int OVEN_TEMPERATURE = 220;
        public const BiscuitCondition MIXTURE = BiscuitCondition.Mixture;
        public const BiscuitCondition EXTRUDER = BiscuitCondition.Extruder;
        public const BiscuitCondition STAMPER = BiscuitCondition.Stamper;
        public const BiscuitCondition OVEN = BiscuitCondition.Oven;
        public const BiscuitCondition POT = BiscuitCondition.Pot;

        public MotorService motorService;
        public Biscuit firstBiscuit;
        public Biscuit secondBiscuit;
        public Biscuit thirdBiscuit;
        public Biscuit fourthBiscuit;
        public Biscuit fifthBiscuit;
        public Biscuit sixthBiscuit;

        [SetUp]
        public void Setup()
        {
            motorService = new MotorService();
        }

        // Test motor is switched on
        [Test]
        public void MotorSwitchOn()
        {
            motorService.StartMotor();

            Assert.AreEqual(MachineSwitch.On, motorService.GetMotorIsSwitchedOn());
        }

        // Test after motor is switched on the conveyor should be ready = initialized
        [Test]
        public void MotorSwitchOnConveyorInitialized()
        {
            int expectedConveyorArrayCount = 0;

            motorService.StartMotor();

            Assert.AreEqual(expectedConveyorArrayCount, motorService.GetConveyorArray().Count);
        }

        // Test after motor is switched on and conveyor started - check pulse
        [Test]
        public void MotorSwitchOnAndConveyorStartedCheckPulse()
        {
            int expectedPulses = 1;
            motorService.StartMotor();

            firstBiscuit = new Biscuit();
            firstBiscuit.Id = Guid.NewGuid();

            motorService.StartConveyor(firstBiscuit);

            Assert.AreEqual(expectedPulses, motorService.GetMotorPulse());
        }

        // Test after motor is switched on and conveyor started - check conveyor array
        [Test]
        public void MotorSwitchOnAndConveyorStartedCheckConveyorArray()
        {
            int expectedConveyorArrayCount = 1;
            motorService.StartMotor();

            firstBiscuit = new Biscuit();
            firstBiscuit.Id = Guid.NewGuid();

            motorService.StartConveyor(firstBiscuit);

            Assert.AreEqual(expectedConveyorArrayCount, motorService.GetConveyorArray().Count);
        }

        // Test after motor is switched on and conveyor started - 5 biscuits
        [Test]
        public void MotorSwitchOnAndConveyorStartedWithBiscuits()
        {
            motorService.StartMotor();

            TestOneBiscuit();

            TestTwoBiscuits();

            TestThreeBiscuits();

            TestFourBiscuits();

            TestFiveBiscuits();

            TestSixBiscuits();
        }
        // Test switch off of the motor when it is switched on
        [Test]
        public void MotorSwitchOffWhenSwitchedOn()
        {
            int expectedPulses = 0;
            int expectedConveyorArrayCount = 0;

            motorService.StartMotor();

            motorService.StopMotor();

            Assert.AreEqual(MachineSwitch.Off, motorService.GetMotorIsSwitchedOn());
            Assert.AreEqual(expectedConveyorArrayCount, motorService.GetConveyorArray().Count);
            Assert.AreEqual(expectedPulses, motorService.GetMotorPulse());
        }
        // Test switch off of the motor without switching it on - throw exception
        [Test]
        public void MotorSwitchOffWithoutSwitchingOn()
        {
            try
            {
                motorService.StopMotor();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The motor should be switched on or paused to be switched off!", ex.Message);
            }            
        }

        // Test switch off of the motor and check the unfinished biscuits - no biscuits at all
        [Test]
        public void MotorSwitchOffAndCheckUnfinishedBiscuitsNoBiscuits()
        {
            int expectedBiscuitsArrayCount = 0;
            int expectedUnifinishedBiscuitsArrayCount = 0;

            motorService.StartMotor();

            motorService.StopMotor();

            Assert.AreEqual(expectedBiscuitsArrayCount, motorService.GetPot().BiscuitsArray.Count);
            Assert.AreEqual(expectedUnifinishedBiscuitsArrayCount, motorService.GetPot().UnfinishedBiscuitsArray.Count);
        }

        // Test switch off of the motor and check the unfinished biscuits - 2 biscuits
        [Test]
        public void MotorSwitchOffAndCheckUnfinishedBiscuitsTwoBiscuits()
        {
            int expectedBiscuitsArrayCountBeforeStopping = 0;
            int expectedBiscuitsArrayCountAfterStopping = 0;

            int expectedUnifinishedBiscuitsArrayCountBeforeStoping = 0;
            int expectedUnifinishedBiscuitsArrayCountAfterStoping = 2;

            motorService.StartMotor();

            for (int i = 0; i < 2; i++)
            {
                CreateBiscuitMixture();
            }

            Assert.AreEqual(expectedBiscuitsArrayCountBeforeStopping, motorService.GetPot().BiscuitsArray.Count);
            Assert.AreEqual(expectedUnifinishedBiscuitsArrayCountBeforeStoping, motorService.GetPot().UnfinishedBiscuitsArray.Count);

            motorService.StopMotor();

            Assert.AreEqual(expectedBiscuitsArrayCountAfterStopping, motorService.GetPot().BiscuitsArray.Count);
            Assert.AreEqual(expectedUnifinishedBiscuitsArrayCountAfterStoping, motorService.GetPot().UnfinishedBiscuitsArray.Count);
        }

        // Test switch off of the motor and check the unfinished biscuits - 4 biscuits and 1 finished
        [Test]
        public void MotorSwitchOffAndCheckUnfinishedBiscuitsFourBiscuits()
        {
            int expectedBiscuitsArrayCountBeforeStopping = 1;
            int expectedUnifinishedBiscuitsArrayCountBeforeStoping = 0;


            int expectedBiscuitsArrayCountAfterStopping = 1;
            int expectedUnifinishedBiscuitsArrayCountAfterStoping = 4;

            motorService.StartMotor();

            for (int i = 0; i < 5; i++)
            {
                CreateBiscuitMixture();
            }

            Assert.AreEqual(expectedBiscuitsArrayCountBeforeStopping, motorService.GetPot().BiscuitsArray.Count);
            Assert.AreEqual(expectedUnifinishedBiscuitsArrayCountBeforeStoping, motorService.GetPot().UnfinishedBiscuitsArray.Count);

            motorService.StopMotor();

            Assert.AreEqual(expectedBiscuitsArrayCountAfterStopping, motorService.GetPot().BiscuitsArray.Count);
            Assert.AreEqual(expectedUnifinishedBiscuitsArrayCountAfterStoping, motorService.GetPot().UnfinishedBiscuitsArray.Count);
        }

        // Test pause of the motor when it is switched on
        [Test]
        public void MotorPauseWhenSwitchedOn()
        {
            motorService.StartMotor();

            motorService.PauseMotor();

            Assert.AreEqual(MachineSwitch.Pause, motorService.GetMotorIsSwitchedOn());
        }

        // Test pause of the motor without switching it on - throw exception
        [Test]
        public void MotorPauseWithoutSwitchingOn()
        {
            try
            {
                motorService.PauseMotor();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The motor should be switched on to be paused!", ex.Message);
            }
        }
        private void TestOneBiscuit()
        {
            firstBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, firstBiscuit.Condition);
        }
        private void TestTwoBiscuits()
        {
            secondBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, secondBiscuit.Condition);
            Assert.AreEqual(STAMPER, firstBiscuit.Condition);
        }
        private void TestThreeBiscuits()
        {
            thirdBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, thirdBiscuit.Condition);
            Assert.AreEqual(STAMPER, secondBiscuit.Condition);
            Assert.AreEqual(OVEN, firstBiscuit.Condition);
        }
        private void TestFourBiscuits()
        {
            fourthBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, fourthBiscuit.Condition);
            Assert.AreEqual(STAMPER, thirdBiscuit.Condition);
            Assert.AreEqual(OVEN, secondBiscuit.Condition);
            Assert.AreEqual(OVEN, firstBiscuit.Condition);
        }
        private void TestFiveBiscuits()
        {
            fifthBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, fifthBiscuit.Condition);
            Assert.AreEqual(STAMPER, fourthBiscuit.Condition);
            Assert.AreEqual(OVEN, thirdBiscuit.Condition);
            Assert.AreEqual(OVEN, secondBiscuit.Condition);
            Assert.AreEqual(POT, firstBiscuit.Condition);
        }
        private void TestSixBiscuits()
        {
            sixthBiscuit = CreateBiscuitMixture();
            Assert.AreEqual(EXTRUDER, sixthBiscuit.Condition);
            Assert.AreEqual(STAMPER, fifthBiscuit.Condition);
            Assert.AreEqual(OVEN, fourthBiscuit.Condition);
            Assert.AreEqual(OVEN, thirdBiscuit.Condition);
            Assert.AreEqual(POT, secondBiscuit.Condition);
            Assert.AreEqual(POT, firstBiscuit.Condition);
        }
        private Biscuit CreateBiscuitMixture()
        {
            var biscuit = new Biscuit();
            biscuit.Id = Guid.NewGuid();

            Assert.AreEqual(MIXTURE, biscuit.Condition);

            motorService.StartConveyor(biscuit);

            return biscuit;
        }
    }
}
