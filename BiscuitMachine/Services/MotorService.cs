using System;
using System.Collections.Generic;
using System.Threading;
using BiscuitMachine.Interfaces;
using BiscuitMachine.Models;
using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Services
{
    public class MotorService : IMotorService
    {
        private Motor motor;
        private Pot pot;
        public MotorService()
        {
            motor = new Motor();

            pot = new Pot();
            pot.BiscuitsArray = new List<Biscuit>();
            pot.UnfinishedBiscuitsArray = new List<Biscuit>();
        }
        public MachineSwitch StartMotor()
        {
            if(motor.Switched == MachineSwitch.Off)
            {
                motor.ConveyorArray = new List<Biscuit>();

                Thread.Sleep(2000);
            }

            motor.Switched = MachineSwitch.On;

            return motor.Switched;
        }

        public void StopMotor()
        {
            if(motor.Switched == MachineSwitch.Off)
            {
                throw new Exception("The motor should be switched on or paused to be switched off!");
            }

            Console.WriteLine("Motor stopped!");

            Thread.Sleep(2000);

            motor.Switched = MachineSwitch.Off;
            motor.Pulse = 0;

            PlaceUnfinishedBiscuitsToPot(pot);
        }

        public void PauseMotor()
        {
            if(motor.Switched != MachineSwitch.On)
            {
                throw new Exception("The motor should be switched on to be paused!");
            }

            Console.WriteLine("Motor paused!");

            motor.Switched = MachineSwitch.Pause;
        }

        private void PlaceUnfinishedBiscuitsToPot(Pot pot)
        {
            pot.UnfinishedBiscuitsArray = new List<Biscuit>();

            if (motor.ConveyorArray.Count > 0)
            {
                do
                {
                    pot.UnfinishedBiscuitsArray.Add(motor.ConveyorArray[0]);

                    motor.ConveyorArray.RemoveAt(0);
                } while (motor.ConveyorArray.Count > 0);
            }

            Console.WriteLine("Unfinished biscuits: " + pot.UnfinishedBiscuitsArray.Count);
        }

        public Pot StartConveyor(Biscuit biscuit)
        {
            // return 1 pulse per revolution
            motor.Pulse++;

            // Put the biscuit on the conveyor
            motor.ConveyorArray.Add(biscuit);

            if (motor.ConveyorArray.Count == 5)
            {
                motor.ConveyorArray[4].Condition = BiscuitCondition.Extruder;
                motor.ConveyorArray[3].Condition = BiscuitCondition.Stamper;
                motor.ConveyorArray[2].Condition = BiscuitCondition.Oven;
                motor.ConveyorArray[1].Condition = BiscuitCondition.Oven;
                motor.ConveyorArray[0].Condition = BiscuitCondition.Pot;

                pot.BiscuitsArray.Add(motor.ConveyorArray[0]);

                motor.ConveyorArray.RemoveAt(0);
            }
            else if (motor.ConveyorArray.Count == 1)
            {
                motor.ConveyorArray[0].Condition = BiscuitCondition.Extruder;
            }
            else if (motor.ConveyorArray.Count == 2)
            {
                motor.ConveyorArray[1].Condition = BiscuitCondition.Extruder;
                motor.ConveyorArray[0].Condition = BiscuitCondition.Stamper;
            }
            else if (motor.ConveyorArray.Count == 3)
            {
                motor.ConveyorArray[2].Condition = BiscuitCondition.Extruder;
                motor.ConveyorArray[1].Condition = BiscuitCondition.Stamper;
                motor.ConveyorArray[0].Condition = BiscuitCondition.Oven;
            }
            else if (motor.ConveyorArray.Count == 4)
            {
                motor.ConveyorArray[3].Condition = BiscuitCondition.Extruder;
                motor.ConveyorArray[2].Condition = BiscuitCondition.Stamper;
                motor.ConveyorArray[1].Condition = BiscuitCondition.Oven;
                motor.ConveyorArray[0].Condition = BiscuitCondition.Oven;
            }

            PrintBiscuits(pot);

            return pot;
        }
        private void PrintBiscuits(Pot pot)
        {
            Console.WriteLine("We have in the conveyor biscuits:");
            for (int i = 0; i < motor.ConveyorArray.Count; i++)
            {
                Console.WriteLine("Biscuit: { " 
                    + "Id: " + motor.ConveyorArray[i].Id 
                    + " - " 
                    + "Condition: " + motor.ConveyorArray[i].Condition
                    + " }");
            }

            Console.WriteLine("On the pot we have " + pot.BiscuitsArray.Count + " biscuits!");
        }
        public MachineSwitch GetMotorIsSwitchedOn()
        {
            return motor.Switched;
        }
        public int GetMotorPulse()
        {
            return motor.Pulse;
        }
        public IList<Biscuit> GetConveyorArray()
        {
            return motor.ConveyorArray;
        }
        public Pot GetPot()
        {
            return pot;
        }
    }
}
