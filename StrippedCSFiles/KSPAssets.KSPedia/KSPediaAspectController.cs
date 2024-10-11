using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaAspectController : MonoBehaviour, ILayoutController, ILayoutSelfController
{
	public enum AspectMode
	{
		None,
		WidthControlsHeight,
		HeightControlsWidth,
		BestFitMin,
		BestFitMax
	}

	public float desiredContentAspect;

	public float borderTop;

	public float borderLeft;

	public float borderRight;

	public float borderBottom;

	public AspectMode aspectMode;

	public float screenBorder;

	public Canvas screenCanvas;

	private RectTransform rt;

	private RectTransform canvasRT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPediaAspectController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLayoutBestFit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetWidthFromHeight(float height = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetHeightFromWidth(float width = -1f)
	{
		throw null;
	}
}
