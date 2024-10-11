using System.Runtime.CompilerServices;

public class UIPartActionResourceItem : UIPartActionItem
{
	protected PartResource resource;

	public PartResource Resource
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetSymCounterpartsAmount(double amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetSymCounterpartsFlowState(bool state)
	{
		throw null;
	}
}
