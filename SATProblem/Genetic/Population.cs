using SATProblem.Problems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SATProblem.Genetic
{
    public class Population
    {
        private readonly Random _random = new Random();

        public int PopulationCount { get; }
        public int Elitism { get; }
        public List<Individual> Individuals { get; private set; } = new List<Individual>();

        public IProblem Problem { get; }
        public Individual GlobalBestIndividual { get; set; }

        public Population(int populationCount, int elitism, IProblem problem)
        {
            PopulationCount = populationCount;
            Elitism = elitism;

            Problem = problem;
            //Items = items;
        }

        public Population(int populationCount, int elitism, IProblem problem, List<Individual> individuals)
        {
            PopulationCount = populationCount;
            Elitism = elitism;

            Problem = problem;
            Individuals = individuals;
            //Items = items;
        }

        public void GenerateInitialPopulation()
        {
            for (int i = 0; i < PopulationCount; i++)
                Individuals.Add(new Individual(Problem.GenomLength));
        }

        public void MakeSelection(int tournamentSize)
        {
            CalculateFitness();

            var winners = new List<Individual>();
            for (int i = 0; i < PopulationCount; i++)
            {
                var winner = MakeTournamentSelection(tournamentSize);
                winners.Add(winner);
            }

            // Set winners to population
            Individuals = winners;
        }

        private void CalculateFitness()
            => CalculateFitness(Individuals);

        private void CalculateFitness(IReadOnlyList<Individual> individuals)
        {
            foreach (var individual in individuals)
                individual.CalculateFitness(Problem);
        }

        private Individual MakeTournamentSelection(int tournamentSize)
        {
            var participants = SelectIndividualsRadomly(tournamentSize);
            var winner = TournamentFight(participants);

            return winner;
        }

        /// <summary>
        /// Select individuals radomly. The individual can be selected only once.
        /// </summary>
        /// <param name="count">Number of individual to select</param>
        /// <returns></returns>
        private IReadOnlyList<Individual> SelectIndividualsRadomly(int count)
        {
            var selectedIndividuals = new List<Individual>();

            var individuals = Individuals.ToList();
            for (int i = 0; i < count; i++)
            {
                var individual = individuals[_random.Next(0, individuals.Count - 1)];

                individuals.Remove(individual);
                selectedIndividuals.Add(individual);
            }

            return selectedIndividuals;
        }

        /// <summary>
        /// Selection
        /// </summary>
        /// <param name="individuals"></param>
        /// <returns></returns>
        private Individual TournamentFight(IReadOnlyList<Individual> individuals)
            => GetFittestIndividual(individuals);

        public void Crossover()
        {
            var elitism = GetFittestIndividuals(Individuals, Elitism);

            double crossoverRate = 0.5;

            int newPopulationCount = 0;
            while (newPopulationCount < PopulationCount)
            {
                var parentA = GetRandomGenom();
                var parentB = GetRandomGenom();

                if (_random.NextDouble() > crossoverRate)
                    Individuals.AddRange(OnePointCrossover(parentA, parentB));
                else
                    Individuals.AddRange(TwoPointCrossover(parentA, parentB));

                newPopulationCount += 2;
            }

            CalculateFitness();
            AddEliteFromPreviousGeneration(elitism);
            //CalculateFitness();
        }

        private List<Individual> OnePointCrossover(bool[] parentA, bool[] parentB)
        {
            var crossoverPoint = _random.Next(1, parentA.Length);

            bool[] offSpringA = new bool[parentA.Length];
            bool[] offSpringB = new bool[parentA.Length];

            for (int i = 0; i < parentA.Length; i++)
                if (crossoverPoint > i)
                {
                    offSpringA[i] = parentA[i];
                    offSpringB[i] = parentB[i];
                }
                else
                {
                    offSpringA[i] = parentB[i];
                    offSpringB[i] = parentA[i];
                }

            return new List<Individual>
                {
                    new Individual(offSpringA),
                    new Individual(offSpringB)
                };
        }

        private List<Individual> TwoPointCrossover(bool[] parentA, bool[] parentB)
        {
            var firstPoint = _random.Next(1, parentA.Length);
            var secondPoint = _random.Next(1, parentA.Length);

            bool[] offSpringA = new bool[parentA.Length];
            bool[] offSpringB = new bool[parentA.Length];

            if (firstPoint > secondPoint)
            {
                var temp = firstPoint;
                firstPoint = secondPoint;
                secondPoint = temp;
            }

            for (int i = firstPoint; i < parentA.Length; i++)
                if (i < firstPoint || i >= secondPoint)
                {
                    offSpringA[i] = parentA[i];
                    offSpringB[i] = parentB[i];
                }
                else
                {
                    offSpringA[i] = parentB[i];
                    offSpringB[i] = parentA[i];
                }

            return new List<Individual>
                {
                    new Individual(offSpringA),
                    new Individual(offSpringB)
                };
        }

        private bool[] GetRandomGenom()
        {
            var index = _random.Next(0, Individuals.Count);
            return Individuals[index].Genom.ToArray();
        }

        public Individual GetFittestIndividual()
            => GetFittestIndividual(Individuals);

        private Individual GetFittestIndividual(IReadOnlyList<Individual> individuals)
            => individuals.OrderByDescending(s => s.Fitness).FirstOrDefault();

        private List<Individual> GetFittestIndividuals(IReadOnlyList<Individual> individuals, int number)
            => individuals.OrderByDescending(s => s.Fitness).Take(number).ToList();

        private void AddEliteFromPreviousGeneration(List<Individual> elitisms)
        {
            if (Elitism > 0)
            {
                var worstIndividuals = Individuals.OrderBy(s => s.Fitness).Take(Elitism).ToList();

                for (var i = 0; i < elitisms.Count; i++)
                {
                    var index = Individuals.IndexOf(Individuals.Where(s => s.Fitness == worstIndividuals[i].Fitness).FirstOrDefault());
                    Individuals[index] = elitisms[i];
                }
            }
        }

        public void Mutate(double mutateRate)
        {
            foreach (var individual in Individuals)
                Mutate(individual, mutateRate);
        }

        private void Mutate(Individual individual, double mutateRate)
        {
            var randomNumber = _random.NextDouble();

            if (randomNumber > mutateRate)
                return;

            var flipBitIndex = _random.Next(0, individual.Genom.Length);
            individual.Genom[flipBitIndex] = !individual.Genom[flipBitIndex];

            //individual.Fitness = Problem.CalculateFitness(individual.Genom);
            individual.CalculateFitness(Problem);
        }

        public Population Clone()
        {
            var clone = new Population(PopulationCount, Elitism, Problem);
            clone.Individuals = Individuals.ToList();

            return clone;
        }

    }
}
