using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UIPanelTransitionToggle : UIPanelTransition
{
	[SerializeField]
	private int expandedIndex;

	[SerializeField]
	private int collapsedIndex;

	public bool expanded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool collapsed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPanelTransitionToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ExpandImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CollapseImmediate()
	{
		throw null;
	}
}
