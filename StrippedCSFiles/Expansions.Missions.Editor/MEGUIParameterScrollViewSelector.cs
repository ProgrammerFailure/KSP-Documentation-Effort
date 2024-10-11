using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ScrollViewSelector]
public class MEGUIParameterScrollViewSelector : MEGUIParameter
{
	public delegate void SelectedElementEvent(int elementIndex);

	public delegate void ExpandEvent(bool isDisplayed);

	public Button buttonExpand;

	public GameObject arrowExpand;

	public GameObject scrollViewParent;

	public RectTransform container;

	public GameObject prefabListElement;

	private List<GameObject> listElements;

	private List<string> listValues;

	private List<SelectedElementEvent> selectedElementEventList;

	private List<ExpandEvent> expandEventList;

	public int FieldValue
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
	public MEGUIParameterScrollViewSelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleExpand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FillList(List<string> newValues)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ButtonCallback(int buttonIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSelectedElement(SelectedElementEvent call)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnExpandView(ExpandEvent call)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsExpanded()
	{
		throw null;
	}
}
