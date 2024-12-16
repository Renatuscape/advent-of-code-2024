using System.Text;

namespace AoC_24_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input-aoc-24-9.txt");
            Console.WriteLine("Part 1 Checksum: " + SolvePart1(input).ToString());
        }

        public static long SolvePart1(string input)
        {
            int[] nums = input.ToCharArray().Select(c => c - '0').ToArray();
            long checkSum = 0L;
            int fileOrder = 0; // 'ID' as described in task. Is set depending on the order in which it appears before moving
            int leftIndex = 0; // Iterator which starts from the left
            int rightIndex = nums.Length - 1; // Iterator which starts from the right

            while (true)
            {
                if (leftIndex % 2 == 0) // Execute if index represents empty space
                {
                    checkSum += UpdateFileCheckSum(nums[leftIndex], leftIndex / 2, ref fileOrder); // Update CheckSum
                    nums[leftIndex] = 0; // Change processed number to 0

                    if (leftIndex >= rightIndex) // Once left iterator meets right iterator, break the loop
                    {
                        break;
                    }

                    leftIndex += 1; // Increase index
                }
                else // Executes if index represents a file
                {
                    if (leftIndex >= rightIndex)
                    {
                        checkSum += (nums[rightIndex] + 1) * nums[rightIndex] / 2;
                        break;
                    }
                    int m = Math.Min(nums[leftIndex], nums[rightIndex]);
                    checkSum += UpdateFileCheckSum(m, rightIndex / 2, ref fileOrder);
                    nums[leftIndex] -= m;
                    nums[rightIndex] -= m;

                    if (nums[leftIndex] == 0) // Skip processed or empty file
                    {
                        leftIndex += 1;
                    }

                    if (nums[rightIndex] == 0) // Skip free space block with no free space
                    {
                        rightIndex -= 2;
                    }
                }

                //Console.WriteLine(string.Join("", nums));
            }
            return checkSum;
        }

        private static long UpdateFileCheckSum(int fileID, long value, ref int fileOrder)
        {
            long newCheckSum = (fileID - 1 + 2 * fileOrder) * fileID / 2 * value;
            fileOrder += fileID;
            return newCheckSum;
        }
    }
}