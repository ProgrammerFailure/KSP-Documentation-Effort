using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Selectable))]
public class UISelectableGridLayoutGroupItem : MonoBehaviour
{
	public Color colorSelected;

	public Color colorDeselected;

	public Transform itemParent;

	public int mySetIndex;

	public bool groupAction;

	private int index;

	private Selectable sel;

	public int Index
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Selectable SelectableComponent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableGridLayoutGroupItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UISelectableGridLayoutGroup selectableGroup, int index)
	{
		throw null;
	}
}
