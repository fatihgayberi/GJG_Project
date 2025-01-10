using System.Collections.Generic;

namespace Wonnasmith.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// verilen listedeki elemanları karıştıp yenidien sıralar
        /// </summary>
        /// <param name="ts"> liste </param>
        /// <typeparam name="T"> listenin tipi </typeparam>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}