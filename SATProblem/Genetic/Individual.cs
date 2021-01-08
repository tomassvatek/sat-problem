using SATProblem.Problems;
using System;

namespace SATProblem.Genetic
{
    public class Individual
    {
        public Individual(int genomLength)
        {
            Genom = new bool[genomLength];
            Random rn = new Random();
            var addRate = 0.5;

            for (int i = 0; i < genomLength; i++)
            {
                var random = rn.NextDouble();
                Genom[i] = random > addRate ? false : true;
            }

            Fitness = 0;
        }

        public Individual(bool[] genes)
        {
            Genom = genes;
        }

        public bool[] Genom { get; }
        public int GenomCount => Genom.Length;
        public int Fitness { get; set; }

        public void CalculateFitness(IProblem problem)
        {
            Fitness = problem.CalculateFitness(Genom);
            //int weight = 0;
            //int cost = 0;
            //for (int i = 0; i < Genom.Length; i++)
            //    if (Genom[i])
            //    {
            //        weight += items[i].Weight;
            //        cost += items[i].Price;
            //    }

            //if (weight > maxWeight)
            //    Fitness = maxWeight - weight;
            //else
            //    Fitness = cost;
        }
    }
}
