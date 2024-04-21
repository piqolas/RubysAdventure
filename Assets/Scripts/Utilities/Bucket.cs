using System;
using System.Collections.Generic;
using piqey.Utilities.Extensions;

namespace piqey.Utilities
{
	public class Bucket<T>
	{
		private readonly T[] _source;
		private readonly List<T> _reserve = new();

		public Bucket(T[] source)
		{
			_source = source;
			_reserve.Capacity = source.Length;

			Fill();
		}

		private void Fill()
		{
			_reserve.Clear();

			List<T> cup = new(_source);
			for (int i = 0; i < cup.Count; i++)
				_reserve.Add(cup.Random(true));
		}

		public T Sample()
		{
			T item = _reserve.Random(true);

			if (_reserve.Count == 0)
				Fill();

			return item;
		}

		public IEnumerable<T> Sample(int num)
		{
			if (num < 1)
				throw new ArgumentOutOfRangeException(nameof(num), "Sample count must equal or exceed 1.");

			for (int i = 0; i < num; i++)
				yield return Sample();
		}
	}
}
