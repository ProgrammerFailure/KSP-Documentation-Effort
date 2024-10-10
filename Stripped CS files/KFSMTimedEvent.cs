using UnityEngine;

public class KFSMTimedEvent : KFSMEvent
{
	public double TimerDuration;

	public KFSMTimedEvent(string name, double time)
		: base(name)
	{
		TimerDuration = time;
		OnCheckCondition = WaitForTimer;
	}

	public bool WaitForTimer(KFSMState currentState)
	{
		return (double)Time.time - currentState.TimeAtStateEnter > TimerDuration;
	}
}
