using System.Collections.Generic;

namespace SATProblem.Genetic
{
    public class GAResult
    {
        public Individual Result { get; set; }
        public List<Population> Generations { get; set; }
    }
}
