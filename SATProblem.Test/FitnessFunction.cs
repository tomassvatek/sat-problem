using SATProblem.Genetic;
using SATProblem.Helpers;
using SATProblem.Problems;
using System.Collections.Generic;
using Xunit;

namespace SATProblem.Test
{
    public class FitnessFunction
    {
        [Fact]
        public void FitnessFunctionTest()
        {
            var formula = new Formula(3, 3);
            formula.Clauses = new int[]
            {
                1, 2, 3,
                -1, -2, -3,
                -1, -2, -3
            };

            formula.Weights = new int[]
            {
                1, 2, 3
            };

            var problem = new ThreeSATProblem(formula);

            var expectedWinner = new Individual(new bool[]
            {
                false, true, true
            });
            var initialPopulation = new List<Individual>
            {
                new Individual(new bool[]
                {
                    false, false, true
                }),
                new Individual(new bool[]
                {
                    true, true, true
                }),
                expectedWinner
            };

            var population = new Population(100, 2, problem, initialPopulation);
            population.MakeSelection(3);
            var winner = population.GetFittestIndividual();

            Assert.Equal(expectedWinner, winner);
        }
    }
}
