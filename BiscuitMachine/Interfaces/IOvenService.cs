using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Interfaces
{
    public interface IOvenService
    {
        public MachineSwitch StartOven(int ovenTemperature);
        public void PauseOven();
        public void StopOven();
        public void PreventingOverheating(int readyBiscuitsCount);
        public MachineSwitch GetOvenIsSwitchedOn();
        public int GetOvenTemperature();
    }
}
