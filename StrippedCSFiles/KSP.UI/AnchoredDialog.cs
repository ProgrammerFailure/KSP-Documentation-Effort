using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public abstract class AnchoredDialog : MonoBehaviour
{
	private float dialogWidth;

	private float dialogHeight;

	private float paddingHorz;

	private float paddingVert;

	private float clampLeft;

	private float clampRight;

	private float clampBottom;

	private float clampTop;

	private float wDist;

	public float standOffDistance;

	public float clampPadding;

	private float zWorldNear;

	private float zWorldFar;

	private float zScreenNear;

	private float zScreenFar;

	protected bool useOpacityFade;

	public bool clampedToScreen;

	public float nearFadeStart;

	public float nearFadeEnd;

	public float farFadeStart;

	public float farFadeEnd;

	protected float opacity;

	protected float lastOpacity;

	protected RectTransform rTrf;

	protected Transform anchor;

	protected Transform trf;

	protected Transform camTrf;

	protected Vector3 wPos;

	protected Vector3 sPos;

	protected bool hover;

	private int framesAtSpawn;

	private Camera refCamera;

	private Camera uiCamera;

	protected AnchoredDialogHost anchorHost;

	protected bool disabled;

	[SerializeField]
	protected Image bgPanel;

	[SerializeField]
	protected TextMeshProUGUI windowTitleField;

	private bool willDestroy;

	protected CanvasGroup canvasGroup;

	protected List<XSelectable> uiControls;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected AnchoredDialog()
	{
		throw null;
	}

	protected abstract void StartThis();

	protected abstract void OnDestroyThis();

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetHover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreatePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnPanelSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachEventHooks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Disable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetBgPanel(Graphic g)
	{
		throw null;
	}

	protected abstract void CreateWindowContent();

	protected abstract string GetWindowTitle();

	protected abstract void OnClickOut();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 getUpAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void setOpacity(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsApplied()
	{
		throw null;
	}
}
