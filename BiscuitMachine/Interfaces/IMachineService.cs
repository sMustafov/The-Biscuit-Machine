using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Interfaces
{
    public interface IMachineService
    {
        public void SwitchOnMachine(int ovenTemperature);
        public void SwitchOffMachine();
        public void PauseMachine();
        public void MachineStartWorking(int ovenTemperature);
        public MachineSwitch GetMachineIsSwitchedOn();
    }
}
