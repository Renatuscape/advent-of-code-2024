namespace AoC_24_1
{
    internal partial class Program
    {
        internal static class PartOne
        {
            internal static void SortLists(List<int> listA, List<int> listB)
            {

                int distance = 0;

                for (int i = 0; i < listA.Count; i++)
                {
                    Console.WriteLine(listA[i] + " - " + listB[i] + " = " + Math.Abs(listA[i] - listB[i]));
                    distance += Math.Abs(listA[i] - listB[i]);
                }
                Console.WriteLine("\nTotal distance: " + distance);
            }
        }
    }
}
