using System.Collections.Generic;

namespace TotalDefenseConcordance
{
    public class Concordance
    {
        public int Count { get; set; }
        public List<int> Sentences { get; set; } = new List<int>();
        public string Word { get; set; }
    }
}
