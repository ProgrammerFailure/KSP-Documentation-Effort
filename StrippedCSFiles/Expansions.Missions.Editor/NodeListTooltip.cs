using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class NodeListTooltip : Tooltip
{
	public TextMeshProUGUI textName;

	public RawImage imageNode;

	public TextMeshProUGUI textInfoBasic;

	public TextMeshProUGUI textDescription;

	public TextMeshProUGUI textRMBHint;

	public PartListTooltipWidget extInfoWidgetPrefab;

	private List<PartListTooltipWidget> extInfoWidgets;

	public Sprite extInfoTestModuleSprite;

	public Sprite extInfoActionModuleSprite;

	public GameObject panelExtended;

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacerPrefab;

	private List<RectTransform> extInfoSpacers;

	private MEBasicNode basicNode;

	private bool hasExtendedInfo;

	private bool hasCreatedExtendedInfo;

	public bool HasExtendedInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NodeListTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(MEBasicNode basicNode, RawImage image)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateExtendedInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateExtInfoElement(MEBasicNode.ExtendedInfo mInfo, bool testModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisplayExtendedInfo(bool display, string rmbHintText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HidePreviousTooltipWidgets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartListTooltipWidget GetNewTooltipWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private RectTransform GetNewTooltipSpacer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
