using System.Runtime.CompilerServices;

public class UIPartActionFieldItem : UIPartActionItem
{
	protected BaseField field;

	protected bool isActiveUnfocused;

	protected float itemRange;

	protected bool removedIfPinned;

	public BaseField Field
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected object Host
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFieldItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsItemValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected BaseField GetField(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool SetSymCounterpartValue(object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FireSymmetryEvents(BaseField baseField, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetFieldValue(object newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckInRange()
	{
		throw null;
	}
}
