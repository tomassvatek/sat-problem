using System;
using System.Text;

namespace SATProblem.Helpers
{
    public class Formula
    {
        private int[] _weights;
        private int[] _clauses;

        public Formula(int variablesCount, int clausesCount)
        {
            VariablesCount = variablesCount;

            ClauseCount = clausesCount;
            _clauses = new int[clausesCount];
        }

        public int VariablesCount { get; }
        public int ClauseCount { get; }

        public int[] Clauses
        {
            get { return _clauses; }
            set
            {
                if (value.Length / 3 != ClauseCount)
                    throw new ArgumentException($"Clauses count '{value.Length}' is not correct. Correct clauses count is '{ClauseCount}'.");

                _clauses = value;
            }
        }

        public int[] Weights
        {
            get { return _weights; }
            set
            {
                if (value.Length != VariablesCount)
                    throw new ArgumentException($"Weights count '{value.Length}' is not equal to variables count '{VariablesCount}'.");

                _weights = value;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Weights: ");
            for (int i = 0; i < Weights.Length; i++)
            {
                var s = ($"{_weights[i]} ");
                sb.Append(s);
            }

            sb.AppendLine();

            for (int i = 0; i < _clauses.Length; i++)
            {
                var s = ($"{_clauses[i]} ");
                sb.Append(s);

                if ((i + 1) % 3 == 0)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
