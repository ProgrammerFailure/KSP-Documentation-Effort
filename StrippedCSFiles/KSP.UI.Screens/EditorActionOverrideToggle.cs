using System.Runtime.CompilerServices;
using TMPro;

namespace KSP.UI.Screens;

public class EditorActionOverrideToggle : UISelectableGridLayoutGroupItem
{
	public delegate void SetOverrideStateDelegate(int groupOverride, bool state);

	public UIButtonToggle toggle;

	public TextMeshProUGUI text;

	public SetOverrideStateDelegate SetOverrideState;

	public int groupOverride
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorActionOverrideToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(int groupOverride, bool on, bool isControl, string tooltipText, bool isGroupAction, int setIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChanged(bool on)
	{
		throw null;
	}
}
