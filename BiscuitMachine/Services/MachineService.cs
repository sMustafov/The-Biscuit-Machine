using System;
using System.Threading;
using BiscuitMachine.Interfaces;
using BiscuitMachine.Models;
using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Services
{
    public class MachineService : IMachineService
    {
        private Machine machine;
        private OvenService ovenService;
        private MotorService motorService;
        private Pot pot;
        public MachineService()
        {
            machine = new Machine();
            pot = new Pot();
        }
        public void SwitchOnMachine(int ovenTemperature)
        {
            if(machine.Switched == MachineSwitch.Pause)
            {
                machine.Switched = MachineSwitch.On;
                motorService.StartMotor();
                ovenService.StartOven(ovenTemperature);

                return;
            }

            // Check if the machine is already switched on
            if (machine.Switched != MachineSwitch.On)
            {
                machine.Switched = MachineSwitch.On;
            }

            // Heating the oven to temperature between 220 and 240 for biscuits
            ovenService = new OvenService();
            var isOvenHeated = ovenService.StartOven(ovenTemperature);

            if (isOvenHeated != MachineSwitch.On)
            {
                throw new Exception("Oven is not heated yet!");
            }

            motorService = new MotorService();
            motorService.StartMotor();
        }

        public void SwitchOffMachine()
        {
            if (machine.Switched == MachineSwitch.Off)
            {
                throw new Exception("The machine should be switched on or paused to be switched off!");
            }

            // Stopping the motor and leaving nothing on the conveyor belt 
            motorService.StopMotor();

            // Stopping the oven
            ovenService.StopOven();

            // Switching off the machine
            Thread.Sleep(2000);
            Console.WriteLine("Machine switched off!");
            machine.Switched = MachineSwitch.Off;
        }
        public void PauseMachine()
        {
            if(machine.Switched != MachineSwitch.On)
            {
                throw new Exception("The machine should be switched on to be paused!");
            }

            machine.Switched = MachineSwitch.Pause;

            ovenService.PauseOven();

            motorService.PauseMotor();
        }
        public void MachineStartWorking(int ovenTemperature)
        {
            if (machine.Switched == MachineSwitch.Pause)
            {
                Console.WriteLine("Machine was paused!");

                SwitchOnMachine(ovenTemperature);
            }
            // Create new biscuit mixture which will go through all the machine steps and then will finish becoming a biscuit
            Biscuit biscuit = new Biscuit();
            biscuit.Id = Guid.NewGuid();

            pot = motorService.StartConveyor(biscuit);

            ovenService.PreventingOverheating(pot.BiscuitsArray.Count);
        }
        public MachineSwitch GetMachineIsSwitchedOn()
        {
            return machine.Switched;
        }
    }
}
