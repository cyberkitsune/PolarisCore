using System.Collections.Generic;
using System.Threading;

namespace Polaris.Lib.Utility
{
	public class MPSCQueue<T> : Queue<T>
	{
		private volatile object _lockObj = new object();
		private ManualResetEventSlim _waitEvent = new ManualResetEventSlim();

		public new void Enqueue(T item)
		{
			lock (_lockObj)
			{
				base.Enqueue(item);
				_waitEvent.Set();
			}
		}

		/// <summary>
		/// Dequeue an object, or wait until until an object is available for dequeuing and dequeue it
		/// </summary>
		/// <returns></returns>
		public T WaitDequeue()
		{
			_waitEvent.Wait();
			T obj;
			lock (_lockObj)
			{
				obj = base.Dequeue();
				if (base.Count == 0)
					_waitEvent.Reset();
			}
			return obj;
		}
	}
}
