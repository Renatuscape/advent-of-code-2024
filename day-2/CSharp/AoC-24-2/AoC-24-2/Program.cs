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
            var reportNumbers = report.Split(' ').ToList();
            bool pass = false;

            for (int i = 0; i < reportNumbers.Count; i++)
            {
                List<string> copyList = new(reportNumbers);
                copyList.Remove(reportNumbers[i]);
                var newReport = string.Empty;

                foreach (var element in copyList)
                {
                    newReport += element + " ";
                }

                if (CheckReport(newReport.TrimEnd()))
                {
                    Console.WriteLine("Report passed when removing " + reportNumbers[i]);
                    pass = true;
                    break;
                }
            }

            return pass;
        }

        public static bool CheckReport(string report)
        {
            var reportNumbers = report.Split(' ');
            int prevTemp = int.Parse(reportNumbers[0]);
            int difference = prevTemp - int.Parse(reportNumbers[1]);
            bool isIncreasing = difference > 0;
           

            for (int i =  1; i < reportNumbers.Length; i++)
            {
                var currentTemp = int.Parse(reportNumbers[i]);
                difference = prevTemp - currentTemp;

                if (currentTemp == prevTemp)
                {
                    return false;
                }
                if (Math.Abs(currentTemp - prevTemp) > 3)
                {
                    return false;
                }
                if (isIncreasing != difference > 0)
                {
                    return false;
                }

                prevTemp = currentTemp;
            }

            return true;
        }
    }
}
