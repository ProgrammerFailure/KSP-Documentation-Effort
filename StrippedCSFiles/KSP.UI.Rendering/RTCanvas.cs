using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Rendering;

public class RTCanvas : MonoBehaviour
{
	[SerializeField]
	private Camera canvasCam;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private int dstLayer;

	private RenderTexture rt;

	private Canvas dstCanvas;

	private CanvasGroup dstCanvasGroup;

	private Camera dstCam;

	private RawImage dstImg;

	[SerializeField]
	private Canvas[] otherCanvases;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RTCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Find Canvases Using Same Camera")]
	public void FindCanvasesUsingCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Setup")]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupRTCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Setup RT")]
	protected void SetupRT()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupRT(int height, int width)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetUIOpacity()
	{
		throw null;
	}
}
