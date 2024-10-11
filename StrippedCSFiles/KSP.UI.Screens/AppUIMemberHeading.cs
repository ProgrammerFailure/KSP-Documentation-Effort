using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

[AppUI_Heading]
public class AppUIMemberHeading : AppUIMember
{
	public AppUI_Control.HorizontalAlignment horizontalAlignment;

	public bool reverseLayoutGroupPadding;

	[SerializeField]
	private RectTransform headingBackground;

	private Vector2 backgroundOffsetMin;

	private Vector2 backgroundOffsetMax;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIMemberHeading()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustHeadingBackground()
	{
		throw null;
	}
}
