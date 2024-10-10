using System;
using System.ComponentModel;
using System.Threading;

public class SafeAbortBackgroundWorker : BackgroundWorker
{
	public Thread workerThread;

	public override void OnDoWork(DoWorkEventArgs e)
	{
		workerThread = Thread.CurrentThread;
		try
		{
			base.OnDoWork(e);
		}
		catch (ThreadAbortException)
		{
			e.Cancel = true;
			Thread.ResetAbort();
		}
		catch (Exception ex2)
		{
			e.Cancel = true;
			e.Result = ex2.Message + "\n" + ex2.StackTrace;
		}
	}

	public bool Abort()
	{
		if (workerThread != null)
		{
			workerThread.Abort();
			workerThread = null;
			return true;
		}
		return false;
	}
}
