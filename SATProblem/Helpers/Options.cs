using CommandLine;

namespace SATProblem.Helpers
{
    public class Options
    {
        [Option('f', "filePath", Required = true, HelpText = "Instance file path")]
        public string FilePath { get; set; }

        [Option('g', "generationCount", Required = true, HelpText = "Generation count")]
        public int GenerationCount { get; set; }

        [Option('p', "populationCount", Required = true, HelpText = "Population count")]
        public int PopulationCount { get; set; }

        [Option('t', "tournamentSize", Required = true, HelpText = "TournamentSize")]
        public int TournamenSize { get; set; }

        [Option('e', "elitnessCount", Required = true, HelpText = "ElitnessCount")]
        public int Elitness { get; set; }

        [Option('m', "mutateRate", Required = true, HelpText = "MutateRate")]
        public double MutateRate { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Verbose", Default = false)]
        public bool Verbose { get; set; }
    }
}
