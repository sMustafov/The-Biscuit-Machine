using System;
using System.Threading;
using BiscuitMachine.Interfaces;
using BiscuitMachine.Models;
using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Services
{
    public class OvenService : IOvenService
    {
        private Oven oven;
        public OvenService()
        {
            oven = new Oven();
        }

        // We should give temperature between 220 - 240 for biscuit
        public MachineSwitch StartOven(int ovenTemperature)
        {
            if(oven.Switched == MachineSwitch.Off)
            {
                if (!CheckIfTemperatureGoodForBiscuits(ovenTemperature))
                {
                    throw new Exception("This temperature is not good for Biscuit! Please, give something between 220 and 240 degree!");
                }

                oven.Temperature = ovenTemperature;

                Thread.Sleep(3000);
            }

            oven.Switched = MachineSwitch.On;

            return oven.Switched;
        }
        private bool CheckIfTemperatureGoodForBiscuits(int ovenTemperature)
        {
            if (ovenTemperature<Biscuit.MIN_TEMPERATURE || ovenTemperature> Biscuit.MAX_TEMPERATURE)
            {
                return false;
            }

            return true;
        }

        public void PreventingOverheating(int readyBiscuitsCount)
        {
            if (readyBiscuitsCount > 0 && readyBiscuitsCount % Oven.MAX_BISCUITS_BEFORE_OVERHEATING == 0)
            {
                Console.WriteLine("Downheating the oven!");

                oven.Switched = MachineSwitch.Off;

                Thread.Sleep(5000);

                oven.Switched = MachineSwitch.On;
            }
        }
        public void PauseOven()
        {
            if (oven.Switched != MachineSwitch.On)
            {
                throw new Exception("The oven should be switched on to be paused!");
            }

            oven.Switched = MachineSwitch.Pause;
        }

        public void StopOven()
        {
            if (oven.Switched == MachineSwitch.Off)
            {
                throw new Exception("The oven should be switched on or paused to be switched off!");
            }

            Console.WriteLine("Oven stopped!");

            Thread.Sleep(2000);

            oven.Temperature = 0;

            oven.Switched = MachineSwitch.Off;
        }
        public MachineSwitch GetOvenIsSwitchedOn()
        {
            return oven.Switched;
        }

        public int GetOvenTemperature()
        {
            return oven.Temperature;
        }
    }
}
