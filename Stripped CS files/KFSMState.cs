using System.Collections.Generic;

public class KFSMState
{
	public string name;

	public double TimeAtStateEnter;

	public int FrameCountAtStateEnter;

	public KFSMUpdateMode updateMode = KFSMUpdateMode.FIXEDUPDATE;

	public KFSMStateChange OnEnter = delegate
	{
	};

	public KFSMCallback OnUpdate = delegate
	{
	};

	public KFSMCallback OnFixedUpdate = delegate
	{
	};

	public KFSMCallback OnLateUpdate = delegate
	{
	};

	public KFSMStateChange OnLeave = delegate
	{
	};

	public List<KFSMEvent> stateEvents;

	public List<KFSMEvent> StateEvents => stateEvents;

	public KFSMState(string name)
	{
		this.name = name;
		stateEvents = new List<KFSMEvent>();
	}

	public bool IsValid(KFSMEvent ev)
	{
		return stateEvents.Contains(ev);
	}

	public void AddEvent(KFSMEvent ev)
	{
		stateEvents.Add(ev);
	}

	public override string ToString()
	{
		return name;
	}
}
