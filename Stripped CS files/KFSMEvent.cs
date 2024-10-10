public class KFSMEvent
{
	public string name;

	public KFSMState GoToStateOnEvent;

	public KFSMUpdateMode updateMode = KFSMUpdateMode.UPDATE;

	public KFSMCallback OnEvent = delegate
	{
	};

	public KFSMEventCondition OnCheckCondition = (KFSMState currentState) => false;

	public KFSMEvent(string name)
	{
		this.name = name;
	}

	public bool IsValid(KFSMState state)
	{
		return state.StateEvents.Contains(this);
	}
}
