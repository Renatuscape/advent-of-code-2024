using System.Xml.Linq;

namespace AoC_24_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] reports = File.ReadAllLines("input-coa-24-2.txt");
            int passes = 0;
            int reportsChecked = 0;
            List<string> failedReports = new();

            foreach (string report in reports)
            {
                reportsChecked++;
                Console.WriteLine($"CHECKING REPORT #{reportsChecked} " + report);

                if (ReportChecker.CheckReport(report))
                {
                    Console.WriteLine($"\t\tReport passed " + report + "\n");
                    passes++;
                }
                else
                {
                    Console.WriteLine($"\t\tReport failed " + report + "\n");
                    failedReports.Add(report);
                }
            }

            Console.WriteLine($"\n{passes} REPORTS PASSED. {failedReports.Count} REPORTS FAILED.");
            Console.WriteLine($"\n\n\n** Activating ADVANCED PROBLEM DAMPENER ***\n");

            foreach (var report in failedReports)
            {
                if (ReportChecker.ActivateAdvancedProblemDampener(report))
                {
                    Console.WriteLine($"\t\tReport passed " + report + "\n");
                    passes++;
                }
                else
                {
                    Console.WriteLine($"\t\tReport failed " + report + "\n");
                }
            }

            Console.WriteLine($"\n{passes} REPORTS PASSED AFTER DAMPENING.");
        }
    }

    internal static class ReportChecker
    {
        public static bool ActivateAdvancedProblemDampener(string report)
        {
            int[] levels = report.Split(' ').Select(int.Parse).ToArray();

            // Try removing each level
            for (int i = 0; i < levels.Length; i++)
            {
                int[] modifiedLevels = levels.Where((_, index) => index != i).ToArray();

                bool increasing = IsIncreasing(modifiedLevels);
                bool decreasing = IsDecreasing(modifiedLevels);

                if ((increasing || decreasing) && IsAdjacentDifferenceValid(modifiedLevels))
                    return true;
            }

            return false;
        }

        static bool IsIncreasing(int[] levels)
        {
            for (int i = 1; i < levels.Length; i++)
            {
                if (levels[i] <= levels[i - 1]) return false;
            }
            return true;
        }

        static bool IsDecreasing(int[] levels)
        {
            for (int i = 1; i < levels.Length; i++)
            {
                if (levels[i] >= levels[i - 1]) return false;
            }
            return true;
        }

        static bool IsAdjacentDifferenceValid(int[] levels)
        {
            for (int i = 1; i < levels.Length; i++)
            {
                int diff = Math.Abs(levels[i] - levels[i - 1]);
                if (diff == 0 || diff > 3) return false;
            }
            return true;
        }

        public static bool CheckReport(string report)
        {
            int[] levels = report.Split(' ').Select(int.Parse).ToArray();


            bool increasing = IsIncreasing(levels);
            bool decreasing = IsDecreasing(levels);

            if ((increasing || decreasing) && IsAdjacentDifferenceValid(levels))
            {
                return true;
            }


            return false;
        }
    }
}
