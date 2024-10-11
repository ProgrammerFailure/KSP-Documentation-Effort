using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using RUI.Icons.Simple;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class RDNodePrefab : MonoBehaviour
{
	public Image arrowT;

	public Image arrowB;

	public Image arrowL;

	public Image arrowR;

	public Image searchHighlight;

	public Image selection;

	public Image circle;

	public TextMeshProUGUI circle_label;

	public GameObject scalar;

	public UIStateButton button;

	public Image techIcon;

	public RectOffset offset;

	public TooltipController_TitleAndText tooltip;

	public float width
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float height
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDNodePrefab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(Icon icon, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(Icon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color GetIconColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetViewable(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScale(float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddInputDelegate(UnityAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAvailablePartsCircle(int parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideAvailablePartsCircle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnselectNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetArrowHeadState(RDNode.Parent parent, bool show, Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Image GetArrowHeadPrefab(RDNode.Anchor anchor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InstantiateArrowHeadAtPos(RDNode.Parent parent, Vector3 pos, Material mat)
	{
		throw null;
	}
}
