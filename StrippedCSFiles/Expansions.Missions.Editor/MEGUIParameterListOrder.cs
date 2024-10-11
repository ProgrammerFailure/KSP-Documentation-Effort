using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ListOrder]
public class MEGUIParameterListOrder : MEGUIParameter
{
	public Button buttonExpand;

	public GameObject arrowExpand;

	public GameObject scrollViewParent;

	public RectTransform container;

	public MEGUIListOrderItem prefabListElement;

	private List<MEGUIListOrderItem> listElements;

	private bool isInitialized;

	public IList FieldValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterListOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetTitle(object item, string fieldTitle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FillList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CleanList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpButton(MEGUIListOrderItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DownButton(MEGUIListOrderItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleExpand()
	{
		throw null;
	}
}
