using SATProblem.Helpers;

namespace SATProblem.Problems
{
    public class ThreeSATProblem : IProblem
    {
        public ThreeSATProblem(Formula formula)
        {
            Formula = formula;
            GenomLength = formula.VariablesCount;
        }

        public Formula Formula { get; }
        public int GenomLength { get; }

        public int CalculateFitness(bool[] genom)
        {
            int sumWeight = 0;
            int notSatisfiableClausesCount = 0;

            for (int i = 0; i < Formula.Clauses.Length; i += 3)
            {
                int endIndex = i + 3;
                var clause = Formula.Clauses[i..endIndex];
                if (!IsClauseSatifiable(clause, genom))
                {
                    notSatisfiableClausesCount++;
                    //break;
                }
            }

            if (notSatisfiableClausesCount == 0)
            {
                for (int i = 0; i < genom.Length; i++)
                {
                    if (genom[i])
                        sumWeight += Formula.Weights[i];
                }
            }
            else
                sumWeight -= notSatisfiableClausesCount;

            return sumWeight;
        }

        private bool IsClauseSatifiable(int[] clause, bool[] genom)
        {
            bool var1Value = GetVarValue(clause[0], genom);
            bool var2Value = GetVarValue(clause[1], genom);
            bool var3Value = GetVarValue(clause[2], genom);

            return var1Value || var2Value || var3Value;
        }

        private bool GetVarValue(int varId, bool[] genom)
        {
            bool flip = varId < 0;
            varId = varId > 0 ? varId : varId * -1;

            return flip ? !genom[varId - 1] : genom[varId - 1];
        }

    }
}
