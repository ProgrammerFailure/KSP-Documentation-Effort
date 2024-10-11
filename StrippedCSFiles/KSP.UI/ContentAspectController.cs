using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class ContentAspectController : MonoBehaviour, ILayoutController, ILayoutSelfController
{
	public enum AspectMode
	{
		None,
		WidthControlsHeight,
		HeightControlsWidth
	}

	public float desiredContentAspect;

	public float borderTop;

	public float borderBottom;

	public float borderLeft;

	public float borderRight;

	public AspectMode aspectMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContentAspectController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRectTransformDimensionsChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLayoutHorizontal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLayoutVertical()
	{
		throw null;
	}
}
