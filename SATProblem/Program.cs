using CommandLine;
using SATProblem.Genetic;
using SATProblem.Helpers;
using SATProblem.Problems;
using System;
using System.Diagnostics;

namespace SATProblem
{
    class Program
    {
        const int MeasurementCount = 3;
        const string DefaultFilePath = @"C:\Users\tomas.svatek\Desktop\HW5\N1\wuf20-78-N1\wuf20-01.mwcnf";
        static readonly GeneticOptions DefaultGenetic = new GeneticOptions(500, 100, 3, 2, 0.2);

        static void Main(string[] args)
        {
            RunSolver(args);

            //Console.WriteLine("3SAT solver finished the job!");
            //Console.ReadKey();
        }

        static void RunSolver(string[] args)
        {
            bool verbose = false;
            string filePath = DefaultFilePath;
            GeneticOptions geneticOptions = DefaultGenetic;

            Parser.Default.ParseArguments<Options>(args)
                 .WithParsed(o =>
                 {
                     filePath = o.FilePath;
                     geneticOptions = new GeneticOptions(o.GenerationCount, o.PopulationCount, o.TournamenSize, o.Elitness, o.MutateRate);
                     verbose = o.Verbose;
                     //geneticOptions = new GeneticOptions(o.GenerationCount, 0, 0, 0, 0);
                 })
                 .WithNotParsed(errors =>
                 {
                     throw new Exception("Parsing program arguments failed.");
                 });

            if (verbose)
            {
                Console.WriteLine("Running solver on instance '{0}' and with arguments '{1}'", filePath, geneticOptions.ToString());
                Console.WriteLine();
            }

            Run3SATSolver(filePath, geneticOptions, verbose);
        }

        static void Run3SATSolver(string filePath, GeneticOptions options, bool verbose = false)
        {
            var instance = SATInstanceLoader.LoadInstance(filePath);

            var problem = new ThreeSATProblem(instance.Formula);

            var stopwatch = new Stopwatch();

            GAResult result = null;
            double relErr = 0;
            double time = 0;

            for (int i = 0; i < MeasurementCount; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                result = GeneticAlgorithm.Solve(problem, options, verbose);

                stopwatch.Stop();

                time += stopwatch.Elapsed.TotalMilliseconds;
                relErr = CalculateRelativeError(instance.OptimalWeight, result.Result.Fitness);
            }

            if (verbose)
            {
                Console.WriteLine();
                PrintConfig(result);
            }

            var relativeErr = CalculateRelativeError(instance.OptimalWeight, result.Result.Fitness) * 100;
            if (verbose)
                Console.WriteLine("Optimal result is '{0}' and approx. '{1}'. Relative error is '{2}%'.", instance.OptimalWeight, result.Result.Fitness, relativeErr);

            if (verbose)
                Console.WriteLine();

            time /= MeasurementCount;
            Console.WriteLine(FormatCsvResult(instance.InstanceId, time, relErr));
        }

        private static void PrintConfig(GAResult result)
        {
            for (var i = 0; i < result.Result.Genom.Length; i++)
            {
                Console.Write("[x{0} {1}] ", i + 1, result.Result.Genom[i]);
            }

            Console.WriteLine();
        }

        static double CalculateRelativeError(double optimalResult, double approximationResult)
        {
            if (optimalResult == 0 && approximationResult == 0)
                return 0;

            return Math.Abs(approximationResult - optimalResult) / Math.Max(optimalResult, approximationResult);
        }

        static string FormatCsvResult(string instanceId, double time, double relativeErr)
            => $"{instanceId};{time};{relativeErr}";

    }
}
