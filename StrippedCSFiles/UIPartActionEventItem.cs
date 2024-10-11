using System.Runtime.CompilerServices;

public class UIPartActionEventItem : UIPartActionItem
{
	protected BaseEvent evt;

	protected bool isExternal;

	protected bool ExternalToEVAOnly;

	protected float itemRange;

	public BaseEvent Evt
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionEventItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseEvent partEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsItemValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckInRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FireSymCounterparts()
	{
		throw null;
	}
}
