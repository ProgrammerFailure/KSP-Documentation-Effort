using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Editor;

public class PartListTooltipMasterController : MonoBehaviour
{
	public int thumbnailSize;

	public LayerMask thumbnailCameraMask;

	public bool useRenderTextureCamera;

	[SerializeField]
	private float thumbnailCameraSize;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	private bool iconHover;

	private bool rectHover;

	private bool hoverAndLocked;

	[NonSerialized]
	public bool pinned;

	[NonSerialized]
	public bool displayExtendedInfo;

	[NonSerialized]
	public PartListTooltip currentTooltip;

	internal DictionaryValueList<GameObject, PartIcon> iconDictionary;

	public float iconSize;

	public static PartListTooltipMasterController Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartListTooltipMasterController()
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
	private void CreateThumbnailCamera(out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void setSafeArea(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setHoverLock(bool hover)
	{
		throw null;
	}
}
