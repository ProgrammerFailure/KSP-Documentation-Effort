using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Group]
public class UIPartActionGroup : UIPartActionItem
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private TextMeshProUGUI groupHeader;

	[SerializeField]
	private RectTransform content;

	[SerializeField]
	private CanvasGroup contentCanvasGroup;

	[SerializeField]
	private LayoutElement contentLayout;

	[SerializeField]
	private Image collapseStateImage;

	[SerializeField]
	private Sprite collapseSprite;

	[SerializeField]
	private Sprite expandSprite;

	private List<Transform> contentItems;

	private string groupName;

	private string groupDisplayName;

	private bool isContentCollapsed;

	private bool initialized;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(string groupName, string groupDisplayName, bool startCollapsed, UIPartActionWindow pawWindow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CollapseGroupToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUIState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddItemToContent(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RescaleContentItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateContentSize()
	{
		throw null;
	}
}
