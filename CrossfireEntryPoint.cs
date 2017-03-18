namespace Crossfire
{
    using System;
    using System.Linq;
    using System.Text;

    public class CrossfireEntryPoint
    {
        public static int[][] jaggedArray;

        public static void Main()
        {
            int[] dimensions = Console.ReadLine()
                                .Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

            int rows = dimensions[0];
            int cols = dimensions[1];

            jaggedArray = new int[rows][];
            int fillNumber = 1;
            for (int row = 0; row < rows; row++)
            {
                jaggedArray[row] = new int[cols];

                int col;
                for (col = 0; col < cols; col++)
                {
                    jaggedArray[row][col] = col + fillNumber;
                }

                fillNumber += col;
            }

            string inputLine = Console.ReadLine().Trim();
            while (inputLine != "Nuke it from orbit")
            {
                string[] commandInfo = inputLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int row = int.Parse(commandInfo[0]);
                int col = int.Parse(commandInfo[1]);
                int radius = int.Parse(commandInfo[2]);

                if (!InputIsValid(row, col))
                {
                    inputLine = Console.ReadLine().Trim();

                    continue;
                }

                RemoveTheColsElements(row, col, radius);

                RemoveTheElementsFromTheRow(row, col, radius);

                RemoveTheEmptyRowsFromTheJaggedArray();

                inputLine = Console.ReadLine().Trim();
            }

            PrintJaggedArray();
        }

        private static void RemoveTheEmptyRowsFromTheJaggedArray()
        {
            if (!jaggedArray.Any(a => a.Length == 0))
            {
                return;
            }

            int rowsOfTheNewJaggedArray = 0;
            for (int currRow = 0; currRow < jaggedArray.Length; currRow++)
            {
                if (jaggedArray[currRow].Length == 0)
                {
                    continue;
                }

                rowsOfTheNewJaggedArray++;
            }

            int[][] jaggedArrayHolder = new int[rowsOfTheNewJaggedArray][];
            int newArrIndex = 0;
            for (int currRow = 0; currRow < jaggedArray.Length; currRow++)
            {
                if (jaggedArray[currRow].Length == 0)
                {
                    continue;
                }

                jaggedArrayHolder[newArrIndex] = jaggedArray[currRow];
                newArrIndex++;
            }

            jaggedArray = jaggedArrayHolder.ToArray();
        }

        private static void RemoveTheElementsFromTheRow(int row, int col, int radius)
        {
            if (row < 0 || row >= jaggedArray.Length)
            {
                return;
            }

            int minCol = (col - radius) >= 0 ? (col - radius) : 0;
            int maxCol = (col + radius) < jaggedArray[row].Length ? (col + radius) : (jaggedArray[row].Length - 1);

            if (minCol > maxCol)
            {
                return;
            }

            int start = minCol;
            int count = maxCol - minCol;

            int[] arrHolder = jaggedArray[row];

            int[] newArray = RemovePortionOfTheArray(arrHolder, start, count);

            jaggedArray[row] = newArray;
        }

        private static void RemoveTheColsElements(int row, int col, int radius)
        {
            if (col < 0 || col >= jaggedArray.Max(a => a.Length))
            {
                return;
            }

            int minRow = (row - radius) >= 0 ? (row - radius) : 0;
            int maxRow = (row + radius) < jaggedArray.Length ? (row + radius) : (jaggedArray.Length - 1);

            for (int currRow = minRow; currRow <= maxRow; currRow++)
            {
                if (currRow == row)
                {
                    continue;
                }

                int[] arrHolder = jaggedArray[currRow];

                if (col >= arrHolder.Length)
                {
                    continue;
                }

                int[] newArray = RemovePortionOfTheArray(arrHolder, col, 0);

                jaggedArray[currRow] = newArray;
            }
        }

        /// <summary>
        /// Removes portion of the array
        /// </summary>
        /// <param name="arrHolder">The array you want to manipulate</param>
        /// <param name="start">The starting point which is included</param>
        /// <param name="count">The count from the start of the elements you want to remove where 0 is equal to one element</param>
        /// <returns></returns>
        private static int[] RemovePortionOfTheArray(int[] arrHolder, int start, int count)
        {
            int newArrLen = arrHolder.Length - (count + 1);
            int[] toReturn = new int[newArrLen];

            int indexOfReturnedArr = 0;
            for (int currElement = 0; currElement < arrHolder.Length; currElement++)
            {
                if (currElement >= start && currElement <= (start + count))
                {
                    continue;
                }

                toReturn[indexOfReturnedArr] = arrHolder[currElement];

                indexOfReturnedArr++;
            }

            return toReturn;
        }

        private static bool InputIsValid(int row, int col)
        {
            bool toReturn = true;
            
            if ((row < 0 ||
                row >= jaggedArray.Length) &&
                (col < 0 ||
                col >= jaggedArray.Max(a => a.Length)))
            {
                toReturn = false;
            }

            return toReturn;
        }

        public static void PrintJaggedArray()
        {
            StringBuilder result = new StringBuilder();

            for (int row = 0; row < jaggedArray.Length; row++)
            {
                for (int col = 0; col < jaggedArray[row].Length; col++)
                {
                    result.Append(jaggedArray[row][col]);

                    if (col + 1 != jaggedArray[row].Length)
                    {
                        result.Append(" ");
                    }
                }

                if (row + 1 != jaggedArray.Length)
                {
                    result.AppendLine();
                }
            }

            Console.WriteLine(result.ToString());
        }
    }
}
