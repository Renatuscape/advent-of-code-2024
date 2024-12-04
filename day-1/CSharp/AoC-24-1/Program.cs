namespace AoC_24_1
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input-coa-24-1.txt");

            List<int> listA = new();
            List<int> listB = new();

            for (int i = 0; i < lines.Length; i++)
            {
                var locationIDs = lines[i].Split("   ");

                listA.Add(int.Parse(locationIDs[0]));
                listB.Add(int.Parse(locationIDs[1]));
            }

            listA.Sort();
            listB.Sort();

            // PART ONE
            PartOne.SortLists(listA, listB);
        }
    }
}
