using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbAppFrame : MonoBehaviour
{
	public TextMeshProUGUI appName;

	public TextMeshProUGUI appTitle;

	public TextMeshProUGUI label;

	public Image header;

	[NonSerialized]
	public Button headerBtn;

	[NonSerialized]
	public PointerClickHandler headerClickHandler;

	public UIList scrollList;

	public RectTransform anchorTop;

	public RectTransform anchorBottom;

	public RectTransform anchorPieChart;

	public RectTransform anchorPieChartLeft;

	private ApplicationLauncherButton appLauncherButton;

	private RectTransform rectTransform;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbAppFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string appTitle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reposition()
	{
		throw null;
	}
}
