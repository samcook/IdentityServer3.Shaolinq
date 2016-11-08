﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer3.Shaolinq.Serialization
{
	internal static class AsyncHelper
	{
		private static readonly TaskFactory MyTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

		public static void RunSync(Func<Task> func)
		{
			MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
		}

		public static TResult RunSync<TResult>(Func<Task<TResult>> func)
		{
			return MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
		}
	}
}