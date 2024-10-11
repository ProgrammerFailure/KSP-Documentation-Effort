using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ManeuverNodeEditorTab : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public ManeuverNodeEditorTabPosition tabPosition;

	public Sprite tabIconOn;

	public Sprite tabIconOff;

	public string tabTooltipCaptionActive;

	public string tabTooltipCaptionInactive;

	public bool tabManagesCaption;

	public string tabName;

	public float updateCooldownSeconds;

	public ManeuverNodeEditorManager mannodeEditorManager;

	private float timeSinceLastUIRefresh;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ManeuverNodeEditorTab()
	{
		throw null;
	}

	public abstract void SetInitialValues();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsTabInteractable()
	{
		throw null;
	}

	public abstract void UpdateUIElements();

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Setup(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setMouseOver(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData evtData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData evtData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WrapperUpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Update()
	{
		throw null;
	}
}
