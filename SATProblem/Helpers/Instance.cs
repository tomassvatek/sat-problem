using System;

namespace SATProblem.Helpers
{
    public class Instance
    {
        public Instance(string instanceId, Formula formula, int optimalWeight, int[] optimalEvaluation)
        {
            InstanceId = instanceId;
            Formula = formula;
            OptimalWeight = optimalWeight;

            if (optimalEvaluation == null || optimalEvaluation.Length != formula.VariablesCount)
                //Console.WriteLine($"Optimal evaluation variables count '{optimalEvaluation?.Length}' is not equal to the formula variable count '{formula.VariablesCount}'.");
                throw new ArgumentException($"Optimal evaluation variables count '{optimalEvaluation.Length}' is not equal to the formula variable count '{formula.VariablesCount}'.");

            OptimalEvaluation = optimalEvaluation;
        }

        public string InstanceId { get; }
        public Formula Formula { get; }
        public int OptimalWeight { get; }
        public int[] OptimalEvaluation { get; }
    }
}
