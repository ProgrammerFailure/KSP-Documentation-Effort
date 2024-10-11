using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UISelectableListItem : MonoBehaviour
{
	private int index;

	public int Index
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISelectableListItem()
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
	public virtual void SetupListItem(UISelectableList selectableList, int index)
	{
		throw null;
	}
}
