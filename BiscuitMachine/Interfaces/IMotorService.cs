using System.Collections.Generic;
using BiscuitMachine.Models;
using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Interfaces
{
    public interface IMotorService
    {
        public MachineSwitch StartMotor();
        public void StopMotor();
        public void PauseMotor();
        public Pot StartConveyor(Biscuit biscuit);
        public MachineSwitch GetMotorIsSwitchedOn();
        public int GetMotorPulse();
        public IList<Biscuit> GetConveyorArray();
        public Pot GetPot();
    }
}
