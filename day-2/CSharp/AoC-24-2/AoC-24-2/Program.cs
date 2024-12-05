namespace AoC_24_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input-coa-24-2.txt");
            int passes = 0;
            int reportsChecked = 0;

            foreach (string line in lines)
            {
                reportsChecked++;
                Console.WriteLine($"CHECKING REPORT #{reportsChecked} " + line);

                if (ReportChecker.CheckReport(line, out var problemIndex))
                {
                    Console.WriteLine($"\t\tReport passed" + line + "\n");
                    passes++;
                }
                else
                {
                    Console.WriteLine($"\t\tReport failed " + line + "\n");

                    var newReport = ReportChecker.ActivateProblemDampener(line, problemIndex);

                    if (ReportChecker.CheckReport(newReport, out var x))
                    {
                        Console.WriteLine($"\t\tDampened Report passed " + line + "\n");
                        passes++;
                    }
                }
            }

            Console.WriteLine($"\n{passes} REPORTS PASSED");
        }
    }

    internal static class ReportChecker
    {
        public static string ActivateProblemDampener(string report, int problemIndex)
        {
            var reportNumbers = report.Split(' ').ToList();
            reportNumbers.Remove(reportNumbers[problemIndex]);

            report = "";

            foreach (var element in reportNumbers)
            {
                report += element + " ";
            }

            return report.TrimEnd();
        }

        public static bool CheckReport(string report, out int problemIndex)
        {
            var reportNumbers = report.Split(' ');
            int prevTemp = int.Parse(reportNumbers[0]);
            int difference = prevTemp - int.Parse(reportNumbers[1]);
            bool isIncreasing = difference > 0;
           

            for (int i =  1; i < reportNumbers.Length; i++)
            {
                problemIndex = i;
                var currentTemp = int.Parse(reportNumbers[i]);
                difference = prevTemp - currentTemp;

                Console.WriteLine($"\t\tThe difference between {prevTemp} and {currentTemp} is {difference}. Temperature is {(isIncreasing ? "increasing" : "decreasing")}");

                if (currentTemp == prevTemp)
                {
                    Console.WriteLine($"\t\t!! Current temp equalled previous temp");
                    return false;
                }
                if (Math.Abs(currentTemp - prevTemp) > 3)
                {
                    Console.WriteLine($"\t\t!! Temp varied by more than 3");
                    return false;
                }
                if (isIncreasing != difference > 0)
                {
                    Console.WriteLine($"\t\t!! Temperature trend changed!");
                    return false;
                }

                prevTemp = currentTemp;
            }

            problemIndex = -1;
            return true;
        }
    }
}
