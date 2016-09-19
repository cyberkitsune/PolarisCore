﻿using System;
using System.Collections.Generic;

namespace Polaris.Lib.Extensions
{
    public static class IListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random((int)DateTime.Now.Ticks);

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
