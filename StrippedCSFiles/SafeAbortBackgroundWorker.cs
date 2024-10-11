using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

public class SafeAbortBackgroundWorker : BackgroundWorker
{
	private Thread workerThread;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SafeAbortBackgroundWorker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDoWork(DoWorkEventArgs e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Abort()
	{
		throw null;
	}
}
