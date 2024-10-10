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

	public string tabName = "";

	public float updateCooldownSeconds = 1f;

	public ManeuverNodeEditorManager mannodeEditorManager;

	public float timeSinceLastUIRefresh;

	public ManeuverNodeEditorTab()
	{
	}

	public abstract void SetInitialValues();

	public virtual bool IsTabInteractable()
	{
		return InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE);
	}

	public abstract void UpdateUIElements();

	public void Setup(Transform parent)
	{
		base.transform.SetParent(parent);
		RectTransform obj = base.transform as RectTransform;
		obj.anchoredPosition = Vector3.zero;
		obj.anchoredPosition3D = Vector3.zero;
		obj.anchorMin = Vector3.zero;
		obj.anchorMax = Vector3.one;
		obj.localScale = Vector3.one;
		obj.sizeDelta = Vector2.zero;
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		SetInitialValues();
	}

	public void setMouseOver(bool state)
	{
		mannodeEditorManager.SetMouseOverGizmo(state);
	}

	public void OnPointerEnter(PointerEventData evtData)
	{
		if (mannodeEditorManager.SelectedManeuverNode != null)
		{
			setMouseOver(state: true);
		}
	}

	public void OnPointerExit(PointerEventData evtData)
	{
		if (!mannodeEditorManager.MouseWithinTool)
		{
			setMouseOver(state: false);
		}
	}

	public void WrapperUpdateUIElements()
	{
		timeSinceLastUIRefresh += Time.deltaTime;
		if (mannodeEditorManager.IsActive && timeSinceLastUIRefresh > updateCooldownSeconds)
		{
			UpdateUIElements();
			timeSinceLastUIRefresh = 0f;
		}
	}

	public void Update()
	{
		WrapperUpdateUIElements();
	}
}
