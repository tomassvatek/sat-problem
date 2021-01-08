using SATProblem.Helpers;
using SATProblem.Problems;
using System;
using System.Collections.Generic;

namespace SATProblem.Genetic
{
    public static class GeneticAlgorithm
    {
        public static GAResult Solve(
            IProblem problem,
            GeneticOptions options,
            bool verbose)
        {
            var generations = new List<Population>();
            Individual bestGenerationIndividual = null;

            var population = new Population(options.PopulationCount, options.Elitness, problem);
            population.GenerateInitialPopulation();

            for (int i = 0; i < options.GenerationCount; i++)
            {
                if (verbose)
                    Console.WriteLine("Generation '{0}' from '{1}'. Best individual has fitness '{2}'", i, options.GenerationCount, bestGenerationIndividual?.Fitness ?? 0);

                population.MakeSelection(options.TournamenSize);

                population.Crossover();

                population.Mutate(options.MutateRate);

                var bestPopulation = population.GetFittestIndividual();
                bestGenerationIndividual = GetBestIndividual(bestGenerationIndividual, bestPopulation);

                population.GlobalBestIndividual = bestGenerationIndividual;
                generations.Add(population.Clone());
            }

            return new GAResult
            {
                Result = bestGenerationIndividual,
                Generations = generations
            };
        }

        private static Individual GetBestIndividual(Individual bestGeneration, Individual bestPopulation)
        {
            if (bestGeneration == null)
                return bestPopulation;

            if (bestPopulation.Fitness > bestGeneration.Fitness)
                return bestPopulation;
            else
                return bestGeneration;
        }

    }
}
