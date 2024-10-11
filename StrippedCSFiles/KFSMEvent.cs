using System.Runtime.CompilerServices;

public class KFSMEvent
{
	public string name;

	public KFSMState GoToStateOnEvent;

	public KFSMUpdateMode updateMode;

	public KFSMCallback OnEvent;

	public KFSMEventCondition OnCheckCondition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KFSMEvent(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsValid(KFSMState state)
	{
		throw null;
	}
}
