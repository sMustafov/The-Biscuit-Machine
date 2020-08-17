using System;
using BiscuitMachine.Models.Enums;

namespace BiscuitMachine.Models
{
    public class Biscuit
    {
        public const int MIN_TEMPERATURE = 220;

        public const int MAX_TEMPERATURE = 240;
        public Guid Id { get; set; }
        public BiscuitCondition Condition { get; set; }
    }
}
