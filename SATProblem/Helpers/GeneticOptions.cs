namespace SATProblem.Helpers
{
    public class GeneticOptions
    {
        public GeneticOptions(
            int generationCount,
            int populationCount,
            int tournamentSize,
            int elitness,
            double mutateRate)
        {
            GenerationCount = generationCount;
            PopulationCount = populationCount;
            TournamenSize = tournamentSize;
            Elitness = elitness;
            MutateRate = mutateRate;
        }

        public int GenerationCount { get; }
        public int PopulationCount { get; }
        public int TournamenSize { get; }
        public int Elitness { get; }
        public double MutateRate { get; }

        public override string ToString()
        {
            return $"G: {GenerationCount} :: P: {PopulationCount} :: T: {TournamenSize} :: E: {Elitness} :: M: {MutateRate}";
        }
    }
}
