using System;
using System.Collections.Generic;

class Program
{
    static int n = 4;
    static List<string[]> ans = new List<string[]>();
    
    static int[] q;
    static bool[] col;
    static bool[] d1;
    static bool[] d2;
    static void Solve(int row)
    {
        if (row == n)
        {
            string[] board = new string[n];

            for (int i = 0; i < n; i++)
            {
                char[] s = new string('.', n).ToCharArray();
                s[q[i]] = 'Q';
                board[i] = new string(s);
            }

            ans.Add(board);
            return;
        }

        for (int c = 0; c < n; c++)
        {
            if (col[c] || d1[row - c + n - 1] || d2[row + c])
                continue;

            q[row] = c;
            col[c] = d1[row - c + n - 1] = d2[row + c] = true;

            Solve(row + 1);

            col[c] = d1[row - c + n - 1] = d2[row + c] = false;
        }
    }

    static void Main()
    {
        q = new int[n];
        col = new bool[n];
        d1 = new bool[2 * n - 1];
        d2 = new bool[2 * n - 1];

        Solve(0);
        
        for (int i = 0; i < ans.Count; i++)
        {
            Console.WriteLine("Solution " + (i + 1));

            foreach (string row in ans[i])
                Console.WriteLine(row);

            Console.WriteLine();
        }
        
        Console.WriteLine("Solution cnt: " + ans.Count);
    }
}