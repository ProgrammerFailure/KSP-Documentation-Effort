using System;
using UnityEngine;

namespace ns13;

public class PartListTooltipMasterController : MonoBehaviour
{
	public int thumbnailSize = 128;

	public LayerMask thumbnailCameraMask;

	public bool useRenderTextureCamera;

	[SerializeField]
	public float thumbnailCameraSize = 34f;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	public bool iconHover;

	public bool rectHover;

	public bool hoverAndLocked;

	[NonSerialized]
	public bool pinned;

	[NonSerialized]
	public bool displayExtendedInfo;

	[NonSerialized]
	public PartListTooltip currentTooltip;

	public DictionaryValueList<GameObject, PartIcon> iconDictionary = new DictionaryValueList<GameObject, PartIcon>();

	public float iconSize = 50f;

	public static PartListTooltipMasterController Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (useRenderTextureCamera)
		{
			CreateThumbnailCamera(out thumbnailCamera, thumbnailCameraSize, thumbnailCameraMask, out thumbnailRenderTexture, thumbnailSize, 24);
			thumbnailCamera.enabled = false;
		}
	}

	public void OnDestroy()
	{
		for (int i = 0; i < iconDictionary.Count; i++)
		{
			iconDictionary.At(i).DestroyIcon();
		}
		iconDictionary.Clear();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void CreateThumbnailCamera(out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth)
	{
		camRef = new GameObject("Tooltip Camera").AddComponent<Camera>();
		camRef.orthographic = true;
		camRef.orthographicSize = camSize;
		camRef.cullingMask = layerMask.value;
		camRef.farClipPlane = 295f;
		camRef.clearFlags = CameraClearFlags.Color;
		camRef.transform.position = new Vector3(0f, -2000f, -300f);
		camRef.allowHDR = false;
		rtRef = new RenderTexture(rtSize, rtSize, rtDepth);
		camRef.targetTexture = rtRef;
	}

	public void HideTooltip()
	{
	}

	public void setSafeArea(bool st)
	{
		if (st)
		{
			InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS | ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_PAD_PICK_PLACE, "PartTooltips_extendedInfoSafeArea");
		}
		else
		{
			InputLockManager.RemoveControlLock("PartTooltips_extendedInfoSafeArea");
		}
	}

	public void setHoverLock(bool hover)
	{
		if (hover && !hoverAndLocked)
		{
			setSafeArea(st: false);
			InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS | ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_PAD_PICK_PLACE, "PartTooltips_ExtendedInfoHover");
			hoverAndLocked = true;
		}
		if (!hover && hoverAndLocked)
		{
			InputLockManager.RemoveControlLock("PartTooltips_ExtendedInfoHover");
			hoverAndLocked = false;
		}
	}
}
