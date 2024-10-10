using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UITreeViewItem : MonoBehaviour
{
	public LayoutElement insetSpace;

	public float levelInsetSize;

	public Button expandButton;

	public Sprite spriteExpanded;

	public Sprite spriteCollapsed;

	public Button backgroundButton;

	public Color colorUnselected = Color.white;

	public Color colorSelected = Color.white;

	public TextMeshProUGUI label;

	public Action onSelect;

	public Action onExpand;

	public void Setup(UITreeView treeView, UITreeView.Item treeViewItem, int level, string text, Action onSelect)
	{
		Setup(treeView, treeViewItem, level, expandable: false, expanded: false, text, null, onSelect);
	}

	public void Setup(UITreeView treeView, UITreeView.Item treeViewItem, int level, bool expandable, bool expanded, string text, Action onExpand, Action onSelect)
	{
		insetSpace.minWidth = (float)level * levelInsetSize;
		if (!expandable)
		{
			UnityEngine.Object.Destroy(expandButton.gameObject);
		}
		else
		{
			(expandButton.targetGraphic as Image).sprite = (expanded ? spriteExpanded : spriteCollapsed);
		}
		label.text = text;
		backgroundButton.onClick.AddListener(delegate
		{
			treeView.SelectItem(treeViewItem, fireCallback: true);
		});
		if (onExpand != null)
		{
			this.onExpand = onExpand;
			expandButton.onClick.AddListener(delegate
			{
				onExpand();
			});
		}
	}
}
