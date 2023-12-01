﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CacaSudoku.Util;

namespace CacaSudoku
{

    internal static class Util
    {
        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public class Pair<T1, T2>
        {

            public Pair(T1 i, T2 j)
            {
                First = i;
                Second = j;
            }

            public T1 First { get; set; }
            public T2 Second { get; set; }
        }
    }

    internal class Sudoku
    {

        private (int, Boolean)[][] puzzle = new (int, Boolean)[9][];
        private int[,] evalMatrix = new int[2, 9];

        public Sudoku((int, Boolean)[] first, (int, Boolean)[] second, (int, Boolean)[] third, (int, Boolean)[] fourth, (int, Boolean)[] fifth, (int, Boolean)[] sixth, (int, Boolean)[] seventh, (int, Boolean)[] eight, (int, Boolean)[] ninth)
        {
            puzzle[0] = first;
            puzzle[1] = second;
            puzzle[2] = third;
            puzzle[3] = fourth;
            puzzle[4] = fifth;
            puzzle[5] = sixth;
            puzzle[6] = seventh;
            puzzle[7] = eight;
            puzzle[8] = ninth;
        }

        public Sudoku((int, Boolean)[][] puzzle)
        {
            this.puzzle = puzzle;
        } 

        public (int, Boolean)[][] Puzzle
        {
            get { return puzzle; }
        }

        /// <summary>
        /// Method <c>Swap</c> swaps the two given indexes of puzzle p;
        /// If the boolean is False you can swap the indexes.
        /// </summary>
        /// <param name="p">Which block you want to swap in</param>
        /// <param name="i">First number you want to swap</param>
        /// <param name="j">Second number you want to swap</param>
        public void Swap(int p, int i, int j)
        {
            if (this.puzzle[p][i].Item2 == false && this.puzzle[p][j].Item2 == false)
            {
                (this.puzzle[p][i], this.puzzle[p][j]) = (this.puzzle[p][j], this.puzzle[p][i]);
            }
        }

        /*  Representation of how the puzzle array is created.
         * 
         *  [0][0, 1, 2]   [1][0, 1, 2]  [2][0, 1, 2]
         *  [0][3, 4, 5]   [1][3, 4, 5]  [2][3, 4, 5]
         *  [0][6, 7, 8]   [1][6, 7, 8]  [2][6, 7, 8]
         *
         *  [3][0, 1, 2]   [4][0, 1, 2]  [5][0, 1, 2]
         *  [3][3, 4, 5]   [4][3, 4, 5]  [5][3, 4, 5]
         *  [3][6, 7, 8]   [4][6, 7, 8]  [5][6, 7, 8]
         *
         *  [6][0, 1, 2]   [7][0, 1, 2]  [8][0, 1, 2]
         *  [6][3, 4, 5]   [7][3, 4, 5]  [8][3, 4, 5]
         *  [6][6, 7, 8]   [7][6, 7, 8]  [8][6, 7, 8]
        */

       
        /// <summary>
        /// 
        /// </summary>
        private void Evaluate()
        {
            /*int[][] rows = new int[3][] {new int[3] { 0, 1, 2 }, new int[3] { 3, 4, 5 }, new int[3] { 6, 7, 8 } };
            int[][] columns = new int[3][] { new int[3] { 0, 3, 6 }, new int[3] { 1, 4, 7 }, new int[3] { 2, 5, 8 } };
            List<int> Row2 = new List<int> { 3, 4, 5 };
            List<int> Row3 = new List<int> { 6, 7, 8 };
            List<int> Column1 = new List<int> { 0, 3, 6 };
            List<int> Column2 = new List<int> { 1, 4, 7 };
            List<int> Column3 = new List<int> { 2, 5, 8 };

            int total = 0;
            foreach (int i in Row1)
            {
                foreach (int j in Row1)
                {
                    puzzle[i][j];
                }
            }*/

            int[,] puzzleArray =
            {
                {puzzle[0][0].Item1, puzzle[0][1].Item1, puzzle[0][2].Item1, puzzle[1][0].Item1, puzzle[1][1].Item1, puzzle[1][2].Item1, puzzle[2][0].Item1, puzzle[2][1].Item1, puzzle[2][2].Item1}, // row 0
                {puzzle[0][3].Item1, puzzle[0][4].Item1, puzzle[0][5].Item1, puzzle[1][3].Item1, puzzle[1][4].Item1, puzzle[1][5].Item1, puzzle[2][3].Item1, puzzle[2][4].Item1, puzzle[2][5].Item1}, // row 1
                {puzzle[0][6].Item1, puzzle[0][7].Item1, puzzle[0][8].Item1, puzzle[1][6].Item1, puzzle[1][7].Item1, puzzle[1][8].Item1, puzzle[1][6].Item1, puzzle[0][7].Item1, puzzle[0][8].Item1}, // row 2
                {puzzle[3][0].Item1, puzzle[3][1].Item1, puzzle[3][2].Item1, puzzle[4][0].Item1, puzzle[4][1].Item1, puzzle[4][2].Item1, puzzle[5][0].Item1, puzzle[5][1].Item1, puzzle[5][2].Item1}, // row 3
                {puzzle[3][3].Item1, puzzle[3][4].Item1, puzzle[3][5].Item1, puzzle[4][3].Item1, puzzle[4][4].Item1, puzzle[4][5].Item1, puzzle[5][3].Item1, puzzle[5][4].Item1, puzzle[5][5].Item1}, // row 4
                {puzzle[3][6].Item1, puzzle[3][7].Item1, puzzle[3][8].Item1, puzzle[4][6].Item1, puzzle[4][7].Item1, puzzle[4][8].Item1, puzzle[5][6].Item1, puzzle[5][7].Item1, puzzle[5][8].Item1}, // row 5
                {puzzle[6][0].Item1, puzzle[6][1].Item1, puzzle[6][2].Item1, puzzle[7][0].Item1, puzzle[7][1].Item1, puzzle[7][2].Item1, puzzle[8][0].Item1, puzzle[8][1].Item1, puzzle[8][2].Item1}, // row 6
                {puzzle[6][3].Item1, puzzle[6][4].Item1, puzzle[6][5].Item1, puzzle[7][3].Item1, puzzle[7][4].Item1, puzzle[7][5].Item1, puzzle[8][3].Item1, puzzle[8][4].Item1, puzzle[8][5].Item1}, // row 7
                {puzzle[6][6].Item1, puzzle[6][7].Item1, puzzle[6][8].Item1, puzzle[7][6].Item1, puzzle[7][7].Item1, puzzle[7][8].Item1, puzzle[8][6].Item1, puzzle[8][7].Item1, puzzle[8][8].Item1}  // row 8
            };

            for (int row = 0; row < 9; row++)
            {                 
                int[] Row = Enumerable.Range(0, puzzleArray.GetUpperBound(1) + 1).Select(i => puzzleArray[row, i]).ToArray();
                evalMatrix[0, row] = CountIncorrect(Row);
            }

            for (int col = 0; col < 9; col++)
            {
                int[] column = new int[9];
                for(int row = 0; row < 9; row++)
                {
                    column[row] = puzzleArray[row,col];
                }

                evalMatrix[1, col] = CountIncorrect(column);
            }
            
        }

        public int CountIncorrect(int[] puzzelstukjes)
        {

            HashSet<int> seen = new HashSet<int>();
            int incorrectCount = 0;

            foreach (var value in puzzelstukjes)
            {
                if(!seen.Add(value))
                {
                   incorrectCount++;
                }
            }

            return incorrectCount;
        }

        // updates the evaluation
        public void Update()
        {

        }

        /// <summary>
        /// Method <c>Generate</c> randomizes the remaining zero's to random numbers between 1-9 that are not yet in the block.
        /// </summary>
        public void Generate() // fills the sudoku with the missing numbers for each block
        {
            Random rand = new Random();

            // check if number is in puzzle and if yes, remove from list
            for (int i = 0; i < puzzle.Count(); i++)
            {
                List<int> numberList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                List<int> exceptList = puzzle[i].Select(x => x.Item1).ToList();

                numberList = numberList.Except(exceptList).ToList();

                // replace zeros with numbers from the randomized list, remove when placed.
                for (int j = 0; j < puzzle[i].Count(); j++)
                {
                    if (puzzle[i][j].Item1 == 0)
                    {
                        int t = rand.Next(numberList.Count);
                        puzzle[i][j].Item1 = numberList[t];
                        numberList.Remove(numberList[t]);

                    }
                }

            }

            Evaluate();
        }

        /// <summary>
        /// Method <c>ToString</c> converts the Sudoku to a string
        /// </summary>
        /// <returns>String of the sudoku</returns>
        public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");

            Array.ForEach(puzzle, ((int, Boolean)[] p) =>
            {
                Array.ForEach(p, ((int, Boolean) i) =>
                {
                    sb.Append(i.Item1);
                    sb.Append(" ");
                });
            });

            sb.Append(" ");

            return sb.ToString();
        }


        /// <summary>
        /// Method <c>FromString</c> generates a sudoku object from a string.
        /// </summary>
        /// <param name="str">The string to generate sudoku from</param>
        /// <returns>Sudoku object</returns>
        public static Sudoku FromString(string str)
        {
            // Remove any whitespace from the input string
            str = str.Replace(" ", "");

            // Use the SplitInParts method to split the string into chunks of 9 characters
            var chunks = Util.SplitInParts(str, 9);

            // Initialize a 2D array to store the puzzle
            (int, Boolean)[][] pussie = new (int, Boolean)[9][];

            // Iterate through the chunks and convert them to arrays of integers
            for (int i = 0; i < 9; i++)
            {
                pussie[i] = new (int, Boolean)[9];
                int[] number = chunks.ElementAt(i).Select(c => int.Parse(c.ToString())).ToArray();
                int index = 0;
                Array.ForEach(number, (int hello) =>
                {
                    pussie[i][index] = (hello, (hello != 0));
                    index++;
                });
            }

            // Create and return a new Sudoku object with the initialized puzzle
            return new Sudoku(pussie[0], pussie[1], pussie[2], pussie[3], pussie[4], pussie[5], pussie[6], pussie[7], pussie[8]);
        }

    }
}