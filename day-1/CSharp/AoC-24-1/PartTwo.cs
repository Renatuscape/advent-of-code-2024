namespace AoC_24_1
{
    internal partial class Program
    {
        internal static class PartTwo
        {
            internal static void CalculateSimilarity(List<int> listA, List<int> listB)
            {
                int similarityScore = 0;

                foreach (int i in listA)
                {
                    int repetitions = 0;
                    foreach (int j in listB)
                    {
                        if (j == i)
                        {
                            repetitions ++;
                        }
                    }
                    similarityScore += i * repetitions;
                }

                Console.WriteLine("\nTotal similarity score:" + similarityScore);
            }
        }
    }
}
