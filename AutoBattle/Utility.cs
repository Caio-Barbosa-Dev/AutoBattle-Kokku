using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{

    /// <summary>
    /// Generic Static class, to store many utility methods
    /// </summary>
    public static class Utility
    {

        public static void Shuffle<T>(this IList<T> list)
        {

            System.Random rng = new System.Random();

            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

        }

    }
}
