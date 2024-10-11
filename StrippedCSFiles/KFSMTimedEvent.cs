using System.Runtime.CompilerServices;

public class KFSMTimedEvent : KFSMEvent
{
	public double TimerDuration;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KFSMTimedEvent(string name, double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool WaitForTimer(KFSMState currentState)
	{
		throw null;
	}
}
