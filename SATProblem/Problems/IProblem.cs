namespace SATProblem.Problems
{
    public interface IProblem
    {
        int GenomLength { get; }
        int CalculateFitness(bool[] genom);
    }
}
