using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionResourcePriority : UIPartActionItem
{
	[SerializeField]
	private TextMeshProUGUI txtPriority;

	[SerializeField]
	private TextMeshProUGUI txtPriorityOffset;

	[SerializeField]
	private Button btnDec;

	[SerializeField]
	private Button btnInc;

	[SerializeField]
	private Button btnReset;

	private int resPriority;

	private int resPriorityOffset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourcePriority()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDecClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIncClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResetClick()
	{
		throw null;
	}
}
