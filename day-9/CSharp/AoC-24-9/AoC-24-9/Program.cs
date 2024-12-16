using System.Text;

namespace AoC_24_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input-aoc-24-9.txt");
            Console.WriteLine("Part 1 Checksum: " + SolvePart1(input).ToString());
            Console.WriteLine("Part 2 Checksum: " + SolvePart2(input).ToString());
        }

        public static long SolvePart1(string input)
        {
            int[] nums = input.ToCharArray().Select(c => c - '0').ToArray();
            long checkSum = 0L;
            int fileID = 0; // 'ID' as described in task. Is set depending on the order in which it appears before moving
            int leftIndex = 0; // Iterator which starts from the left
            int rightIndex = nums.Length - 1; // Iterator which starts from the right

            while (true)
            {
                if (leftIndex % 2 == 0) // Execute if index represents empty space
                {
                    checkSum += UpdateFileCheckSum(nums[leftIndex], leftIndex / 2, ref fileID); // Update CheckSum
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
                    checkSum += UpdateFileCheckSum(m, rightIndex / 2, ref fileID);
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

        public static long SolvePart2(string input)
        {
            int[] nums = input.ToCharArray().Select(c => c - '0').ToArray();
            int[] numsCopy = nums.ToArray();
            long result = 0L;
            int fileID = 0;
            int index = 0;

            while (index < nums.Length)
            {
                if (index % 2 == 0)
                {
                    int v = index / 2;
                    if (nums[index] == 0)
                    {
                        fileID += numsCopy[index];
                    }

                    result += UpdateFileCheckSum(nums[index], index / 2, ref fileID);
                    nums[index] = 0;
                    index += 1;
                }
                else
                {
                    bool found = false;
                    int j = -1;
                    var loopTo = index;
                    for (j = nums.Length - 1; j >= loopTo; j -= 2)
                    {
                        if (nums[j] > 0 && nums[j] <= nums[index])
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        result += UpdateFileCheckSum(nums[j], j / 2, ref fileID);
                        nums[index] -= nums[j];
                        nums[j] = 0;
                        if (nums[index] == 0)
                        {
                            index += 1;
                        }
                    }
                    else
                    {
                        fileID += nums[index];
                        index += 1;
                    }
                }
            }
            return result;
        }

        private static long UpdateFileCheckSum(int fileSize, long filePosition, ref int fileID)
        {
            long newCheckSum = (fileSize - 1 + 2 * fileID) * fileSize / 2 * filePosition;
            fileID += fileSize;
            return newCheckSum;
        }
    }
}