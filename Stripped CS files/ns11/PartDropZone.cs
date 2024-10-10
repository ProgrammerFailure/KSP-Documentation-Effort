using ns12;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

[RequireComponent(typeof(PointerClickHandler))]
public class PartDropZone : TooltipController
{
	public string tooltipTitle;

	public string tooltipDefault;

	public string tooltipNoPart;

	public Tooltip_TitleAndText tooltipPrefab;

	public PointerClickHandler handler;

	public Callback<AvailablePart> onAddPart = delegate
	{
	};

	public static PartDropZone Instance { get; set; }

	public override void Awake()
	{
		Instance = this;
		handler = GetComponent<PointerClickHandler>();
		handler.onPointerUp.AddListener(ButtonInputDelegate);
		base.Awake();
	}

	public override void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		base.OnDestroy();
	}

	public void ButtonInputDelegate(PointerEventData eventData)
	{
		if (EditorLogic.SelectedPart == null)
		{
			Debug.Log("PartDropZone: Needs to have a part selected to save");
		}
		else
		{
			onAddPart(EditorLogic.SelectedPart.partInfo);
		}
	}

	public override void OnTooltipSpawned(Tooltip instance)
	{
		Tooltip_TitleAndText obj = instance as Tooltip_TitleAndText;
		string text = Localizer.Format(tooltipDefault);
		if (EditorLogic.SelectedPart == null)
		{
			text = text + "\n" + Localizer.Format(tooltipNoPart);
		}
		obj.title.text = tooltipTitle;
		obj.label.text = text;
	}
}
