using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UISelectableListItem_StateButton : UISelectableListItem
{
	[Serializable]
	public class ButtonState
	{
		public Sprite normal;

		public Sprite highlight;

		public Sprite active;

		public Sprite disabled;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ButtonState()
		{
			throw null;
		}
	}

	public ButtonState selected;

	public ButtonState unSelected;

	private Image image;

	private Button button;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem_StateButton()
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
