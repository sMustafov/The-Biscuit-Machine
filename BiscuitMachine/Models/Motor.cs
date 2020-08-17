using System.Collections.Generic;

namespace BiscuitMachine.Models
{
    public class Motor : Switch
    {
        public int Pulse { get; set; }
        public IList<Biscuit> ConveyorArray { get; set; }
    }
}
