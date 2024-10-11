using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Category]
public class MEGUIParameterGroup : MEGUIParameter
{
	public MEGUIParameterGroupHeader header;

	public VerticalLayoutGroup mainLayout;

	public RectTransform containerChilden;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	[SerializeField]
	private List<Color> headerColors;

	private Image headerImage;

	protected BaseAPFieldList groupFields;

	private int _offset;

	private int _depth;

	public BaseAPField Field
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseAPFieldList GroupFields
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Offset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int Depth
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
	public MEGUIParameterGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter Create(BaseAPFieldList categoryFields, Transform parent, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter Create(Transform parent, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int SortParametersByOrder(MEGUIParameter p1, MEGUIParameter p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CollapseGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonCollapsePressed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortChilden()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnParameterGroupReset()
	{
		throw null;
	}
}
