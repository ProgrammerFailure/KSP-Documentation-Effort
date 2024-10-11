using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class KFSMState
{
	public string name;

	public double TimeAtStateEnter;

	public int FrameCountAtStateEnter;

	public KFSMUpdateMode updateMode;

	public KFSMStateChange OnEnter;

	public KFSMCallback OnUpdate;

	public KFSMCallback OnFixedUpdate;

	public KFSMCallback OnLateUpdate;

	public KFSMStateChange OnLeave;

	protected List<KFSMEvent> stateEvents;

	public List<KFSMEvent> StateEvents
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KFSMState(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsValid(KFSMEvent ev)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEvent(KFSMEvent ev)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
