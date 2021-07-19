using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchGuard.RoverApi.Exercise.Extensions
{
    public static class EnumerableExtensions
    {
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int chunkSize)
		{
			if (list == null)
				throw new ArgumentException("You must provide an enumerable to chunk");

			if (chunkSize <= 0)
				throw new ArgumentException("The chunkSize must be greater than 0.");

			while (list.Any())
			{
				yield return list.Take(chunkSize);
				list = list.Skip(chunkSize);
			}
		}
	}
}
