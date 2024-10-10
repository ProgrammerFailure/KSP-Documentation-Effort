using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UIHoverText : XHoverable
{
	[SerializeField]
	public TextMeshProUGUI textTargetForHover;

	[SerializeField]
	public Selectable interactibleTargetControl;

	[SerializeField]
	public bool RequireInteractable = true;

	[SerializeField]
	public string hoverText = "";

	[SerializeField]
	public bool clearTextOnHoverExit = true;

	public void UpdateTargetControl(TextMeshProUGUI textLabel)
	{
		textTargetForHover = textLabel;
	}

	public void UpdateTargetControl(Selectable interactibleTarget)
	{
		interactibleTargetControl = interactibleTarget;
	}

	public void UpdateText()
	{
		OnHover(null, null);
	}

	public new void Start()
	{
		base.Start();
		interactibleTargetControl = GetComponent<Selectable>();
		base.onPointerEnter += OnHover;
		base.onPointerExit += OnUnhover;
		OnUnhover(null, null);
	}

	public new void OnDisable()
	{
		base.OnDisable();
		OnUnhover(null, null);
	}

	public void OnHover(XHoverable arg1, PointerEventData arg2)
	{
		if ((interactibleTargetControl == null || !RequireInteractable || interactibleTargetControl.interactable) && textTargetForHover != null)
		{
			textTargetForHover.text = hoverText;
		}
	}

	public void OnUnhover(XHoverable arg1, PointerEventData arg2)
	{
		if (clearTextOnHoverExit && textTargetForHover != null)
		{
			textTargetForHover.text = "";
		}
	}
}
