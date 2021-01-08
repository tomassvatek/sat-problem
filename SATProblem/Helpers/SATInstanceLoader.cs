using System;
using System.Collections.Generic;
using System.IO;

namespace SATProblem.Helpers
{
    public static class SATInstanceLoader
    {
        private const int FormulaStartRowNumber = 12;
        private const int ThreeSAT = 3;
        private const string InstancePrefix = "wuf";

        public static Instance LoadInstance(string filePath)
        {
            // Figure out var. and claus. count
            var dirName = Path.GetDirectoryName(filePath).Split("\\")[^1].Trim();
            var removeWuf = dirName.Replace(InstancePrefix, string.Empty);
            var split = removeWuf.Trim().Split("-");

            var clauses = new List<int>();
            int variableCount = int.Parse(split[0]);
            int clauseCount = int.Parse(split[1]);

            string solInstanceName = string.Empty;

            using var sr = new StreamReader(filePath);
            string line;

            var formula = new Formula(variableCount, clauseCount);

            int rowNumber = 1;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("c SAT instance"))
                    solInstanceName = line.Split(" ")[3].Split("/")[1];

                if (line.StartsWith("w"))
                    formula.Weights = ParseWeights(line).ToArray();

                if (rowNumber < FormulaStartRowNumber)
                {
                    rowNumber++;
                    continue;
                }

                var clausule = ParseClausule(line);
                clauses.AddRange(clausule);

                rowNumber++;
            }

            formula.Clauses = clauses.ToArray();

            var (weight, config) = LoadResult(filePath, dirName, Path.GetFileNameWithoutExtension(solInstanceName));

            return new Instance(solInstanceName, formula, weight, config);
        }

        private static (int weight, int[] config) LoadResult(string filePath, string dirName, string solInstanceName)
        {
            var solFileName = dirName[0..^1] + "-opt.dat";
            var solDirPath = Path.GetDirectoryName(filePath).Split("\\")[0..^1];

            var solFilePath = Path.Combine(Path.Combine(solDirPath), solFileName);

            if (!File.Exists(solFilePath))
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("Solution file '{0}' not found.", solFilePath);
                //Console.ForegroundColor = ConsoleColor.White;
                //return (0, null);
                throw new Exception($"Solution file '{solFilePath}' not found.");
            }

            return ParseSolution(solFilePath, solInstanceName);
        }

        private static (int weight, int[] config) ParseSolution(string solFilePath, string solInstanceName)
        {
            using var sr = new StreamReader(solFilePath);

            int weight = 0;
            var config = new List<int>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string lineName = line.Split(" ")[0];
                if (lineName.Equals(solInstanceName))
                {
                    var removeIdAndZero = line.Split(" ")[1..^1];
                    weight = int.Parse(removeIdAndZero[0]);
                    for (int i = 1; i < removeIdAndZero.Length; i++)
                    {
                        if (int.TryParse(removeIdAndZero[i], out var value) && value != 0)
                            config.Add(value);
                    }

                    break;
                }
            }


            return (weight, config.ToArray());
        }

        private static List<int> ParseWeights(string line)
        {
            var weights = new List<int>();
            var split = line.Trim().Split("  ");

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length > 1)
                {
                    var split2 = split[i].Split(" ");
                    for (int j = 0; j < split2.Length; j++)
                    {
                        if (int.TryParse(split2[j], out var value) && value != 0)
                            weights.Add(value);
                    }
                    //weights.Add(int.Parse(split[i]));
                }
                else
                {
                    if (int.TryParse(split[i], out var value))
                        weights.Add(value);
                }
            }

            return weights;
        }

        private static int[] ParseClausule(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentNullException(line);

            var removeWhitespace = line.Trim();
            var split = removeWhitespace.Split(" ");

            var clause = new int[ThreeSAT];
            for (int i = 0; i < split.Length - 1; i++)
            {
                clause[i] = int.Parse(split[i]);
            }

            return clause;
        }
    }
}
