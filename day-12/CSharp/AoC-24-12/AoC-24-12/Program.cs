namespace AoC_24_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool useExampleInput = false;
            string input;

            if (useExampleInput)
            {
                input = File.ReadAllText("example.txt");
            }
            else
            {
                input = File.ReadAllText("input.txt");
            }
        }
    }
}
