using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UIListSorter : MonoBehaviour
{
	public delegate void OnSort(int mode, bool asc);

	public UIStateImage[] sortingButtonStates;

	public UIStateImage startSortingMode;

	public bool startSortingAsc;

	private OnSort onSort;

	private UIStateImage activeSortingMode;

	private bool activeSortingAsc;

	private UIStateImage lastSortingMode;

	private bool[] lastValue;

	public int StartSortingIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListSorter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSortingMode(UIStateImage mode, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClickButton(int button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnSortCallback(OnSort del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnSortCallback(OnSort del)
	{
		throw null;
	}
}
