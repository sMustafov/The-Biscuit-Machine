using System.Collections.Generic;

namespace BiscuitMachine.Models
{
    public class Pot
    {
        public IList<Biscuit> BiscuitsArray {get; set;}
        public IList<Biscuit> UnfinishedBiscuitsArray { get; set; }
    }
}
