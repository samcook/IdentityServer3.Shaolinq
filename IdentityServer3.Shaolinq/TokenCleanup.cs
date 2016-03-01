using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer3.Core.Logging;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq
{
	public class TokenCleanup
	{
		private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

		private readonly IIdentityServerOperationalDataAccessModel dataModel;
		private readonly TimeSpan interval;
		private CancellationTokenSource cancellationTokenSource;

		public TokenCleanup(IIdentityServerOperationalDataAccessModel dataModel, int intervalSecs = 60)
		{
			this.dataModel = dataModel;
			this.interval = TimeSpan.FromSeconds(intervalSecs);
		}

		public void Start()
		{
			if (cancellationTokenSource != null)
			{
				throw new InvalidOperationException("Already started. Call Stop first.");
			}

			cancellationTokenSource = new CancellationTokenSource();
			Task.Factory.StartNew(() => Start(cancellationTokenSource.Token));
		}

		public void Stop()
		{
			if (cancellationTokenSource == null)
			{
				throw new InvalidOperationException("Not started.");
			}

			cancellationTokenSource.Cancel();
			cancellationTokenSource = null;
		}

		public async Task Start(CancellationToken cancellationToken)
		{
			Logger.InfoFormat("Starting TokenCleanup with interval {0}", interval);

			while (true)
			{
				try
				{
					await Task.Delay(interval, cancellationToken);
				}
				catch (Exception ex)
				{
					Logger.InfoException("Exception during Task.Delay, stopping", ex);
					break;
				}

				if (cancellationToken.IsCancellationRequested)
				{
					Logger.Info("Cancellation requested");
					break;
				}

				await ClearTokens();
			}
		}

		private async Task ClearTokens()
		{
			try
			{
				using (var scope = DataAccessScope.CreateReadCommitted())
				{
					// TODO is this correct with DateTimeOffsets (can't use Expiry property as it is a local property, not persisted)
					await dataModel.Tokens.DeleteAsync(x => x.ExpiryDateTime < DateTime.UtcNow);

				    await scope.CompleteAsync();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Error clearing tokens", ex);
			}
		}
	}
}