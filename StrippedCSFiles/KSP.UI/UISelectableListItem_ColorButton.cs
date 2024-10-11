using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class UISelectableListItem_ColorButton : UISelectableListItem
{
	[Serializable]
	public class ButtonState
	{
		public Color normal;

		public Color highlight;

		public Color active;

		public Color disabled;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ButtonState()
		{
			throw null;
		}
	}

	public ButtonState selected;

	public ButtonState unSelected;

	private Button button;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem_ColorButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetupListItem(UISelectableList selectableList, int index)
	{
		throw null;
	}
}
