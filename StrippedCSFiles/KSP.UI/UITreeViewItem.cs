using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UITreeViewItem : MonoBehaviour
{
	public LayoutElement insetSpace;

	public float levelInsetSize;

	public Button expandButton;

	public Sprite spriteExpanded;

	public Sprite spriteCollapsed;

	public Button backgroundButton;

	public Color colorUnselected;

	public Color colorSelected;

	public TextMeshProUGUI label;

	public Action onSelect;

	public Action onExpand;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UITreeViewItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(UITreeView treeView, UITreeView.Item treeViewItem, int level, string text, Action onSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(UITreeView treeView, UITreeView.Item treeViewItem, int level, bool expandable, bool expanded, string text, Action onExpand, Action onSelect)
	{
		throw null;
	}
}
