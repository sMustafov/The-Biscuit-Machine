using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Models
{
    public class Oven : Switch
    {
        public const int MAX_BISCUITS_BEFORE_OVERHEATING = 6;
        public int Temperature { get; set; }
        public int PreventedOverHeatingCount { get; set; }
    }
}
